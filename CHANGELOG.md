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
