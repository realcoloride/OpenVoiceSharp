# <img src="https://raw.githubusercontent.com/realcoloride/OpenVoiceSharp/master/openvoicesharp.png" alt="OpenVoiceSharp" width="28" height="28"> OpenVoiceSharp

Agnostic VoIP Voice Chat and Audio Streaming C# library.

## Introduction

>[!WARNING]
>This package is not ready to use and is in work in progress. Please come back later!

**OpenVoiceSharp** is an extremely simple, basic compact library that allows for real time VoIP (Voice over IP) voice chat and audio streaming. It allows for any app or game to embed voice chatting functionality.

**OpenVoiceSharp** utilizes **Opus** as codec under the hood and **RNNoise** for basic (toggleable) noise suppression and **WebRTC** VAD (voice activity detection).

**OpenVoiceSharp** also has a dedicated class (`VoiceUtilities`) for converting PCM to float formats depending on use cases for engines.

## Why did I make this?

I believe that voice chat, proximity or not is an essential functionality for game immersion or multiplayer, or discussion. Though, when searching for a friendly open source/free alternative other than Steam's Voice API or Epic Online Services's Voice API, I could not find any, and to make matters worse, it was extremely difficult to get information around how voice chat/audio streaming essentially worked under the hood, which can make difficult for people to make their own voice chat implementation.

Most alternatives are paid (Vivox, Photon Voice, Dissonance etc...) & are mostly compatible for Unity, which can cause an issue for developers using their own game engine, app stack, or game engines like the Godot Engine. 

So upon learning how to make VoIP myself, I decided to share the knowledge into this library to make sure no one has to ever struggle with VoIP again, because I also believe that implementation for such things should be easy to use and implement.

## Features

- 🕓 Agnostic: no specific engine/environment required!
- ⚡ Easy and friendly to use: all you need is a way to record and playback the audio!
- 🎙️ Basic microphone recorder class: no way to record the audio correctly or easily? `BasicMicrophoneRecorder` does that!
- 💥 Low memory footprint: using **Opus**, the packets are ***tiny***! And **OpenVoiceSharp** aims to be as memory efficient and performant as possible.
- 🎵 Audio streaming favoring: option to encode less for better quality packets for audio streaming and more!
- 😯 Low latency: **OpenVoiceSharp** aims to be as low latency as possible. **One opus frame is only 20ms!**
- 🔊 Customizable bitrate: make audio **crystal crisp** or not, it depends on you! (Supports from 8kbps up to 512kbps)
- 🍃 Basic noise suppression using **RNNoise** (can be toggled)
- 🧪 Basic voice conversion utilies: convert 16 bit PCM to float 32 PCM and so on.

> [!NOTE]  
> **OpenVoiceSharp** is meant to be extremely basic and straightforward. Audio playback, modification (effects or more) and features such as groups, teams, muting should be left to implement by yourself. 
> _**OpenVoiceSharp** just provides a basic way to encode and decode voice packets along with a basic microphone recorder._

## Requirements

- Windows (64 bit)
- .NET 6.0 and higher or support for .NET Standard 2.1
- Visual Studio

> [!WARNING]
> **OpenVoiceSharp** currently only supports Windows 64 bit, atleast the dependencies do.
> I currently do not plan on integrating native support for MacOS, Linux, Android or others but you can compile the libraries yourself and link them.

## Installation & Usage

Everything you need to know or do can be found [in the wiki](https://github.com/realcoloride/OpenVoiceSharp/wiki).

## Contribute

soon enough:tm:

## Troubleshooting

soon enough:tm:

## Licenses & Disclaimer

**OpenVoiceSharp** uses the following libraries, so by using **OpenVoiceSharp**, you accept their license's conditions.

> [!TIP]  
> Most of the libraries used by **OpenVoiceSharp** are MIT licensed, except for WebRTC's VAD, which contains the license from WebRTC.

- [NAudio](https://github.com/naudio/NAudio) - Licensed [MIT](https://github.com/naudio/NAudio/blob/master/license.txt)
- [Opus](https://opus-codec.org/) - Licensed [BSD](https://opus-codec.org/license/)
- [OpusDotNet](https://github.com/mrphil2105/OpusDotNet) - Licensed [MIT](https://github.com/mrphil2105/OpusDotNet/blob/master/LICENSE.md)
- [WebRtcVadSharp](https://github.com/ladenedge/WebRtcVadSharp) & [WebRTC](https://webrtc.org/) - Licensed [MIT](https://github.com/ladenedge/WebRtcVadSharp/blob/main/LICENSE) & [Other](https://webrtc.org/support/license)
- [YellowDogMan.RRNoise.NET](https://github.com/Yellow-Dog-Man/RNNoise.Net) - Licensed [MIT](https://github.com/Yellow-Dog-Man/RNNoise.Net/blob/main/LICENSE) 

_As of this library, just good old MIT._

##### &copy; (real)coloride - 2024 | Licensed MIT
