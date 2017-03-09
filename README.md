# Pomodoro Timer
A timer that helps you with performing "The Pomodoro Technique"®.

![Pomodoro Timer Logo](https://raw.githubusercontent.com/piah/pomodorotimer/master/PomodoroTimer.Docs/logo.png)

## Project Description

### Features

- Lets you track your Pomodoros and has an HTML rendered history
- Lets you configure your Pomodoros
- You can choose between Pomodoro, Small Break, Long Break
- Multi-Language Support (German and English)
- You can configure if there should be a special Action when a Pomodoro has been finished
  For example you could play a sound file or start another program that turns your lighting on
- Easily see the remaining time in the taskbar without having the appliaction on top
- You can have it always on top if you want
- More is coming soon...

## Dependencies

You need to have my MvvmCommons Library for building PomodoroTimer on your own.
You can find it [here](https://github.com/PIaH/Mvvm.Commons).


## Build

```
msbuild -t:Clean,Build
```

## Screenshots

![The Main Screen, where can access all functions](https://raw.githubusercontent.com/piah/pomodorotimer/master/PomodoroTimer.Docs/screenshot_01.png)

![The Settings screen](https://raw.githubusercontent.com/piah/pomodorotimer/master/PomodoroTimer.Docs/screenshot_02.png)

![The history that is rendered into the browser](https://raw.githubusercontent.com/piah/pomodorotimer/master/PomodoroTimer.Docs/screenshot_03.png)

![The notification when you had to interrupt your Pomodoro](https://raw.githubusercontent.com/piah/pomodorotimer/master/PomodoroTimer.Docs/screenshot_04.png)

![The progress of the remaining time in the taskbar icon](https://raw.githubusercontent.com/piah/pomodorotimer/master/PomodoroTimer.Docs/screenshot_05.png)
