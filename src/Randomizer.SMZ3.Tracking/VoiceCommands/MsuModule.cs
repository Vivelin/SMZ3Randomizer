﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Timers;
using Microsoft.Extensions.Logging;
using MSURandomizerLibrary;
using MSURandomizerLibrary.Configs;
using MSURandomizerLibrary.Models;
using MSURandomizerLibrary.Services;
using Randomizer.Abstractions;
using Randomizer.Data.Configuration.ConfigFiles;
using Randomizer.Data.Configuration.ConfigTypes;
using Randomizer.Data.Options;
using Randomizer.Data.Tracking;
using Randomizer.SMZ3.Tracking.Services;

namespace Randomizer.SMZ3.Tracking.VoiceCommands;

/// <summary>
/// Module for tracker stating what the current song is
/// </summary>
public class MsuModule : TrackerModule, IDisposable
{
    private readonly IMsuSelectorService _msuSelectorService;
    private Msu? _currentMsu;
    private readonly string? _msuPath;
    private readonly ICollection<string>? _inputMsuPaths;
    private readonly Timer? _timer;
    private readonly MsuType? _msuType;
    private readonly MsuConfig _msuConfig;
    private readonly string _msuKey = "MsuKey";
    private int _currentTrackNumber;
    private readonly HashSet<int> _validTrackNumbers;
    private Track? _currentTrack;
    private bool _isSetup;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tracker"></param>
    /// <param name="itemService">Service to get item information</param>
    /// <param name="worldService">Service to get world information</param>
    /// <param name="logger"></param>
    /// <param name="msuLookupService"></param>
    /// <param name="msuSelectorService"></param>
    /// <param name="msuTypeService"></param>
    /// <param name="msuConfig"></param>
    public MsuModule(
        TrackerBase tracker,
        IItemService itemService,
        IWorldService worldService,
        ILogger<MsuModule> logger,
        IMsuLookupService msuLookupService,
        IMsuSelectorService msuSelectorService,
        IMsuTypeService msuTypeService,
        MsuConfig msuConfig)
        : base(tracker, itemService, worldService, logger)
    {
        _msuSelectorService = msuSelectorService;
        _msuType = msuTypeService.GetSMZ3MsuType();
        _msuConfig = msuConfig;
        _validTrackNumbers = _msuType!.ValidTrackNumbers;

        if (!File.Exists(tracker.RomPath))
        {
            throw new InvalidOperationException("No tracker rom file found");
        }

        if (string.IsNullOrEmpty(tracker.Rom?.MsuPaths))
        {
            return;
        }

        var romFileInfo = new FileInfo(tracker.RomPath);
        _msuPath = romFileInfo.FullName.Replace(romFileInfo.Extension, ".msu");

        if (!File.Exists(_msuPath))
        {
            return;
        }

        try
        {
            _currentMsu = msuLookupService.LoadMsu(_msuPath, _msuType, false, true, true);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error loading MSU {Path}", _msuPath);
            return;
        }

        if (_currentMsu == null)
        {
            logger.LogWarning("MSU file found but unable to load MSU");
            return;
        }

        // Start reshuffling every minute if requested
        if (tracker.Rom!.MsuRandomizationStyle == MsuRandomizationStyle.Continuous)
        {
            _inputMsuPaths = tracker.Rom!.MsuPaths?.Split("|");
            _timer = new Timer(TimeSpan.FromSeconds(60));
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        tracker.TrackNumberUpdated += TrackerOnTrackNumberUpdated;
        _isSetup = true;

    }

    private void TrackerOnTrackNumberUpdated(object? sender, TrackNumberEventArgs e)
    {
        if (!_validTrackNumbers.Contains(e.TrackNumber)) return;
        _currentTrackNumber = e.TrackNumber;
        if (_currentMsu == null) return;
        _currentTrack =_currentMsu.GetTrackFor(_currentTrackNumber);

        Logger.LogInformation("Current Track: {Track}", _currentTrack?.GetDisplayText() ?? "Unknown");

        if (_currentTrack == null) return;

        var output = GetOutputText();
        TrackerBase.UpdateTrack(_currentMsu, _currentTrack, output);

        // Respond if we have lines to the song number, song name, or msu name
        if (_msuConfig.SongResponses?.TryGetValue(_currentTrack.MsuName ?? "", out var response) == true)
        {
            TrackerBase.Say(response);
        }
        else if (_msuConfig.SongResponses?.TryGetValue(_currentTrack.SongName, out response) == true)
        {
            TrackerBase.Say(response);
        }
        else if (_msuConfig.SongResponses?.TryGetValue(_currentTrackNumber.ToString(), out response) == true)
        {
            TrackerBase.Say(response);
        }

        if (string.IsNullOrEmpty(TrackerBase.Options.MsuTrackOutputPath)) return;
        try
        {
            _ = File.WriteAllTextAsync(TrackerBase.Options.MsuTrackOutputPath, output);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unable to write current track details to {Path}", TrackerBase.Options.MsuTrackOutputPath);
        }

    }

    private string GetOutputText()
    {
        if (_currentMsu == null || _currentTrack == null)
            return "";

        var options = TrackerBase.Options;
        switch (options.MsuTrackDisplayStyle)
        {
            case MsuTrackDisplayStyle.Horizontal:
                {
                    if (!string.IsNullOrEmpty(_currentTrack.DisplayAlbum) || !string.IsNullOrEmpty(_currentTrack.DisplayArtist))
                    {
                        return new MsuDisplayTextBuilder(_currentTrack, _currentMsu)
                            .AddAlbum("{0} - ")
                            .AddTrackTitle("{0}")
                            .AddArtist(" ({0})")
                            .ToString();
                    }
                    else
                    {
                        return new MsuDisplayTextBuilder(_currentTrack, _currentMsu)
                            .AddTrackTitle("{0}")
                            .AddMsuName(" from {0}")
                            .ToString();
                    }
                }

            case MsuTrackDisplayStyle.Vertical:
                {
                    var lines = new List<string>();

                    var creator = string.IsNullOrEmpty(_currentTrack.MsuCreator)
                        ? _currentMsu.DisplayCreator
                        : _currentTrack.MsuCreator;
                    var msu = string.IsNullOrEmpty(_currentTrack.MsuName)
                        ? _currentMsu.DisplayName
                        : _currentTrack.MsuName;
                    lines.Add(string.IsNullOrEmpty(creator)
                        ? $"MSU: {msu}"
                        : $"MSU: {msu} by {creator}");

                    if (!string.IsNullOrEmpty(_currentTrack.DisplayAlbum))
                        lines.Add($"Album: {_currentTrack.DisplayAlbum}");

                    lines.Add($"Song: {_currentTrack.SongName}");

                    if (!string.IsNullOrEmpty(_currentTrack.DisplayArtist))
                        lines.Add($"Artist: {_currentTrack.DisplayArtist}");

                    return string.Join("\r\n", lines);
                }

            case MsuTrackDisplayStyle.HorizonalWithMsu:
                return new MsuDisplayTextBuilder(_currentTrack, _currentMsu)
                    .AddAlbum("{0}: ")
                    .AddTrackTitle("{0}")
                    .AddArtist(" - {0}")
                    .AddMsuNameAndCreator(" (MSU: {0})")
                    .ToString();

            case MsuTrackDisplayStyle.SentenceStyle:
            default:
                return _currentTrack.GetDisplayText(includeMsu: true);
        }
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private GrammarBuilder GetLocationSongRules()
    {
        if (_msuConfig.TrackLocations == null)
        {
            return new GrammarBuilder();
        }

        var msuLocations = new Choices();

        foreach (var track in _msuConfig.TrackLocations)
        {
            if (track.Value == null)
            {
                continue;
            }
            foreach (var name in track.Value)
            {
                msuLocations.Add(new SemanticResultValue(name, track.Key));
            }
        }

        var option1 = new GrammarBuilder()
            .Append("Hey tracker,")
            .OneOf("what's the current song for", "what's the song for", "what's the current theme for", "what's the theme for")
            .Optional("the")
            .Append(_msuKey, msuLocations);

        var option2 = new GrammarBuilder()
            .Append("Hey tracker,")
            .OneOf("what's the current", "what's the")
            .Append(_msuKey, msuLocations)
            .OneOf("song", "theme");

        return GrammarBuilder.Combine(option1, option2);

    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private GrammarBuilder GetLocationMsuRules()
    {
        if (_msuConfig.TrackLocations == null)
        {
            return new GrammarBuilder();
        }

        var msuLocations = new Choices();

        foreach (var track in _msuConfig.TrackLocations)
        {
            if (track.Value == null)
            {
                continue;
            }

            foreach (var name in track.Value)
            {
                msuLocations.Add(new SemanticResultValue(name, track.Key));
            }
        }

        var option1 = new GrammarBuilder()
            .Append("Hey tracker,")
            .Append("what MSU pack is")
            .OneOf("the current song for", "the song for", "the current theme for", "the theme for")
            .Optional("the")
            .Append(_msuKey, msuLocations)
            .Append("from");

        var option2 = new GrammarBuilder()
            .Append("Hey tracker,")
            .Append("what MSU pack is")
            .OneOf("the current", "the")
            .Append(_msuKey, msuLocations)
            .OneOf("song", "theme")
            .Append("from");

        return GrammarBuilder.Combine(option1, option2);

    }

    private string GetTrackText(Track track)
    {
        var parts = new List<string>() { track.SongName };
        if (!string.IsNullOrEmpty(track.DisplayAlbum))
        {
            parts.Add($"from the album {track.DisplayAlbum}");
        }
        else if (!string.IsNullOrEmpty(track.DisplayArtist))
        {
            parts.Add($"by {track.DisplayArtist}");
        }
        if (!string.IsNullOrEmpty(track.MsuName) && TrackerBase.Rom!.MsuRandomizationStyle != null)
        {
            parts.Add($"from MSU Pack {track.MsuName}");
            if (!string.IsNullOrEmpty(track.MsuCreator)) parts.Add($"by {track.MsuCreator}");
        }

        return string.Join("; ", parts);
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private GrammarBuilder GetCurrentSongRules()
    {
        if (_msuConfig.TrackLocations == null)
        {
            return new GrammarBuilder();
        }

        var msuLocations = new Choices();

        foreach (var track in _msuConfig.TrackLocations)
        {
            if (track.Value == null)
            {
                continue;
            }

            foreach (var name in track.Value)
            {
                msuLocations.Add(new SemanticResultValue(name, track.Key));
            }
        }

        return new GrammarBuilder()
            .Append("Hey tracker,")
            .OneOf("what's the current song", "what's currently playing", "what's the current track");

    }

    private GrammarBuilder GetCurrentMsuRules()
    {
        return new GrammarBuilder()
            .Append("Hey tracker,")
            .Append("what MSU pack is")
            .OneOf("the current song from", "the current track from", "the current theme from");
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            var response = _msuSelectorService.CreateShuffledMsu(new MsuSelectorRequest()
            {
                MsuPaths = _inputMsuPaths, OutputMsuType = _msuType, OutputPath = _msuPath, PrevMsu = _currentMsu
            });
            _currentMsu = response.Msu;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Error creating MSU");
        }

        TrackerBase.GameService?.TryCancelMsuResume();
    }

    public void Dispose()
    {
        if (_timer != null)
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public override void AddCommands()
    {
        if (!_isSetup)
        {
            return;
        }

        AddCommand("location song", GetLocationSongRules(), (result) =>
        {
            if (_currentMsu == null)
            {
                TrackerBase.Say(_msuConfig.UnknownSong);
                return;
            }

            var trackNumber = (int)result.Semantics[_msuKey].Value;
            var track = _currentMsu.GetTrackFor(trackNumber);
            if (track != null)
            {
                TrackerBase.Say(_msuConfig.CurrentSong, GetTrackText(track));
            }
            else
            {
                TrackerBase.Say(_msuConfig.UnknownSong);
            }
        });

        AddCommand("location msu", GetLocationMsuRules(), (result) =>
        {
            if (_currentMsu == null)
            {
                TrackerBase.Say(_msuConfig.UnknownSong);
                return;
            }

            var trackNumber = (int)result.Semantics[_msuKey].Value;
            var track = _currentMsu.GetTrackFor(trackNumber);
            if (track?.GetMsuName() != null)
            {
                TrackerBase.Say(_msuConfig.CurrentMsu, track.GetMsuName());
            }
            else
            {
                TrackerBase.Say(_msuConfig.UnknownSong);
            }
        });

        AddCommand("current song", GetCurrentSongRules(), (_) =>
        {
            if (_currentTrack == null)
            {
                TrackerBase.Say(_msuConfig.UnknownSong);
                return;
            }
            TrackerBase.Say(_msuConfig.CurrentSong, GetTrackText(_currentTrack));
        });

        AddCommand("current msu", GetCurrentMsuRules(), (_) =>
        {
            if (_currentTrack == null)
            {
                TrackerBase.Say(_msuConfig.UnknownSong);
                return;
            }
            TrackerBase.Say(_msuConfig.CurrentMsu, _currentTrack.GetMsuName());
        });
    }
}
