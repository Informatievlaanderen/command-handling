## [5.0.1](https://github.com/informatievlaanderen/command-handling/compare/v5.0.0...v5.0.1) (2020-04-08)


### Bug Fixes

* use correct build user ([8845828](https://github.com/informatievlaanderen/command-handling/commit/8845828694834491d0e8700666de8a83bcbc251f))

# [5.0.0](https://github.com/informatievlaanderen/command-handling/compare/v4.2.2...v5.0.0) (2020-04-08)


### chore

* upgrade sql stream store ([#10](https://github.com/informatievlaanderen/command-handling/issues/10)) ([406c873](https://github.com/informatievlaanderen/command-handling/commit/406c8732202b673e9b7cccbd2d2e47d60e45d214))


### BREAKING CHANGES

* Upgrade SqlStreamStore

## [4.2.2](https://github.com/informatievlaanderen/command-handling/compare/v4.2.1...v4.2.2) (2020-03-03)


### Bug Fixes

* force build ([d6a265e](https://github.com/informatievlaanderen/command-handling/commit/d6a265ef936501cd28ffa39b498b3affb4911c90))

## [4.2.1](https://github.com/informatievlaanderen/command-handling/compare/v4.2.0...v4.2.1) (2020-03-03)


### Bug Fixes

* bump netcore to 3.1.2 ([696901e](https://github.com/informatievlaanderen/command-handling/commit/696901e18ae0c6ac1d9c45b576a2f3fe9eee7402))

# [4.2.0](https://github.com/informatievlaanderen/command-handling/compare/v4.1.0...v4.2.0) (2020-01-31)


### Features

* upgrade netcoreapp31 and dependencies ([20ab559](https://github.com/informatievlaanderen/command-handling/commit/20ab559204e5aa2fcff21eeac826d8c81233dafa))

# [4.1.0](https://github.com/informatievlaanderen/command-handling/compare/v4.0.0...v4.1.0) (2019-12-15)


### Features

* upgrade to netcoreapp31 ([a3afa4f](https://github.com/informatievlaanderen/command-handling/commit/a3afa4f841462abe336d443e3edfa57e052a656b))

# [4.0.0](https://github.com/informatievlaanderen/command-handling/compare/v3.2.0...v4.0.0) (2019-11-22)


### Code Refactoring

* upgrade to netcoreapp30 ([afe16a5](https://github.com/informatievlaanderen/command-handling/commit/afe16a5))


### BREAKING CHANGES

* Upgrade to .NET Core 3

# [3.2.0](https://github.com/informatievlaanderen/command-handling/compare/v3.1.0...v3.2.0) (2019-08-22)


### Features

* bump to .net 2.2.6 ([b32747f](https://github.com/informatievlaanderen/command-handling/commit/b32747f))

# [3.1.0](https://github.com/informatievlaanderen/command-handling/compare/v3.0.2...v3.1.0) (2019-06-25)


### Features

* add JsonSerializerSettings for logging in tests ([2cbbe14](https://github.com/informatievlaanderen/command-handling/commit/2cbbe14))

## [3.0.2](https://github.com/informatievlaanderen/command-handling/compare/v3.0.1...v3.0.2) (2019-04-25)

## [3.0.1](https://github.com/informatievlaanderen/command-handling/compare/v3.0.0...v3.0.1) (2019-02-26)


### Bug Fixes

* make AddSqlStreamStore private ([9633a50](https://github.com/informatievlaanderen/command-handling/commit/9633a50))
* make AddSqlStreamStore public ([48ac500](https://github.com/informatievlaanderen/command-handling/commit/48ac500))

# [3.0.0](https://github.com/informatievlaanderen/command-handling/compare/v2.0.1...v3.0.0) (2019-02-26)


### Code Refactoring

* remove finalHandler from CommandHandlerModule ([b7553de](https://github.com/informatievlaanderen/command-handling/commit/b7553de))
* remove StreamStoreCommandHandlerModule ([4847d6b](https://github.com/informatievlaanderen/command-handling/commit/4847d6b))


### Features

* add SqlStreamStorePipe ([e1d9696](https://github.com/informatievlaanderen/command-handling/commit/e1d9696))


### BREAKING CHANGES

* FinalHandler does not exist anymore in CommandHandlerModule, use pipes.
* StreamStoreCommandHandlerModule and RegisterSqlStreamStoreCommandHandler do not
exist anymore. Use SqlStreamStorePipe.

## [2.0.1](https://github.com/informatievlaanderen/command-handling/compare/v2.0.0...v2.0.1) (2019-02-25)


### Bug Fixes

* finally the finalhandler always runs last ([240b69c](https://github.com/informatievlaanderen/command-handling/commit/240b69c))

# [2.0.0](https://github.com/informatievlaanderen/command-handling/compare/v1.4.0...v2.0.0) (2019-02-16)


### Bug Fixes

* fix the order of the piping mechanism ([c701e7d](https://github.com/informatievlaanderen/command-handling/commit/c701e7d))


### Features

* remove finally method ([32bbfcb](https://github.com/informatievlaanderen/command-handling/commit/32bbfcb))


### BREAKING CHANGES

* Finally was removed.
* Handle is now void.

# [1.4.0](https://github.com/informatievlaanderen/command-handling/compare/v1.3.0...v1.4.0) (2019-01-21)


### Features

* register InMemoryStreamStore as IReadonlyStreamStore ([af331e3](https://github.com/informatievlaanderen/command-handling/commit/af331e3))

# [1.3.0](https://github.com/informatievlaanderen/command-handling/compare/v1.2.0...v1.3.0) (2019-01-19)


### Features

* register MsSqlStreamStore as IReadonlyStreamStore ([55c6588](https://github.com/informatievlaanderen/command-handling/commit/55c6588))

# [1.2.0](https://github.com/informatievlaanderen/command-handling/compare/v1.1.0...v1.2.0) (2019-01-15)


### Features

* allow passing in ms sqlstreamstore settings before registering ([2ab8649](https://github.com/informatievlaanderen/command-handling/commit/2ab8649))

# [1.1.0](https://github.com/informatievlaanderen/command-handling/compare/v1.0.0...v1.1.0) (2019-01-08)


### Features

* add autofac based test base class, using xunit ([eccc096](https://github.com/informatievlaanderen/command-handling/commit/eccc096))

# 1.0.0 (2018-12-19)


### Features

* open source with MIT license as 'agentschap Informatie Vlaanderen' ([e824c2c](https://github.com/informatievlaanderen/command-handling/commit/e824c2c))
