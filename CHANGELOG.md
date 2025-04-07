## [10.0.2](https://github.com/informatievlaanderen/command-handling/compare/v10.0.1...v10.0.2) (2025-04-07)


### Bug Fixes

* **deps:** update dependencies ([fd6401a](https://github.com/informatievlaanderen/command-handling/commit/fd6401a5b823a89e94289a4a4d883b15e8127384))

## [10.0.1](https://github.com/informatievlaanderen/command-handling/compare/v10.0.0...v10.0.1) (2025-04-07)


### Bug Fixes

* **deps:** update dependency nodatime to 3.2.2 ([#293](https://github.com/informatievlaanderen/command-handling/issues/293)) ([905212e](https://github.com/informatievlaanderen/command-handling/commit/905212ec7704bd6c82631f42fcbedb53bc9f3461))

# [10.0.0](https://github.com/informatievlaanderen/command-handling/compare/v9.1.1...v10.0.0) (2025-04-07)


### Code Refactoring

* use renovate and nuget + update pipeline ([464def3](https://github.com/informatievlaanderen/command-handling/commit/464def36034bf087000003fe04c3ce0bd249367d))


### BREAKING CHANGES

* update to dotnet 9

## [9.1.1](https://github.com/informatievlaanderen/command-handling/compare/v9.1.0...v9.1.1) (2024-11-27)


### Bug Fixes

* AfterEventTypeStrategy now correctly checks type ([900cd60](https://github.com/informatievlaanderen/command-handling/commit/900cd606b632aa1bd06735abeadd9cb09f4d0cfe))

# [9.1.0](https://github.com/informatievlaanderen/command-handling/compare/v9.0.1...v9.1.0) (2024-11-07)


### Features

* add AnySnapshotStrategy and AllSnapshotStrategy ([7e470c5](https://github.com/informatievlaanderen/command-handling/commit/7e470c52b942555b111cc6935b4a9e230c19cd54))

## [9.0.1](https://github.com/informatievlaanderen/command-handling/compare/v9.0.0...v9.0.1) (2024-03-08)


### Bug Fixes

* **bump:** correct ti release to nuget ([6edb7d0](https://github.com/informatievlaanderen/command-handling/commit/6edb7d0266d7dcfc013b44d9299f2d99caff4074))

# [9.0.0](https://github.com/informatievlaanderen/command-handling/compare/v8.3.0...v9.0.0) (2024-03-08)


### Features

* move to dotnet 8.0.2 ([b180ef0](https://github.com/informatievlaanderen/command-handling/commit/b180ef0ea3fc52708b744ed626ee9298a1e6cd36))


### BREAKING CHANGES

* move to dotnet 8.0.2

# [8.3.0](https://github.com/informatievlaanderen/command-handling/compare/v8.2.0...v8.3.0) (2023-11-29)


### Features

* add IIdempotentCommandHandler ([dd9b7e2](https://github.com/informatievlaanderen/command-handling/commit/dd9b7e271e5f0242f83761188d17f490d8e018f6))

# [8.2.0](https://github.com/informatievlaanderen/command-handling/compare/v8.1.3...v8.2.0) (2023-09-12)


### Features

* custom list comparer which ignores private dotnet List fields ([e3a436a](https://github.com/informatievlaanderen/command-handling/commit/e3a436a46341c1379db4ad6972f3d8a59e34452f))

## [8.1.3](https://github.com/informatievlaanderen/command-handling/compare/v8.1.2...v8.1.3) (2023-09-08)


### Bug Fixes

* snapshot verifier compare collections ([53e4dce](https://github.com/informatievlaanderen/command-handling/commit/53e4dcebc1f3e787479cc6a7ad3e6cd65c1c1985))

## [8.1.2](https://github.com/informatievlaanderen/command-handling/compare/v8.1.1...v8.1.2) (2023-09-08)


### Bug Fixes

* snapshot verifier registrations ([312295f](https://github.com/informatievlaanderen/command-handling/commit/312295fcf26a19ba218c5911a0174c44a724c42d))

## [8.1.1](https://github.com/informatievlaanderen/command-handling/compare/v8.1.0...v8.1.1) (2023-09-08)

# [8.1.0](https://github.com/informatievlaanderen/command-handling/compare/v8.0.0...v8.1.0) (2023-09-06)


### Features

* add snapshot verifier ([d4ca9d5](https://github.com/informatievlaanderen/command-handling/commit/d4ca9d572947223b2930bcf478de28a90f628658))

# [8.0.0](https://github.com/informatievlaanderen/command-handling/compare/v7.1.4...v8.0.0) (2023-02-09)


### Bug Fixes

* build errors ([d098a76](https://github.com/informatievlaanderen/command-handling/commit/d098a76c0918a917d6ccbd5fb6a4ec28d4a3a5f6))
* correct registration microsoft ioc ([3a96aaa](https://github.com/informatievlaanderen/command-handling/commit/3a96aaa46edb10d4776c411aae02a26114f16948))
* revert using ImmutableDictionary ([69b61a6](https://github.com/informatievlaanderen/command-handling/commit/69b61a6f96ba57e6d9ea649e7863304f6bacf4c6))


### Code Refactoring

* remove microsoft and autofac from idempotency ([a941449](https://github.com/informatievlaanderen/command-handling/commit/a941449599afe7dcae3d973984ffaa9105a345c2))


### Features

* add direct registration extension ([d75d5b8](https://github.com/informatievlaanderen/command-handling/commit/d75d5b8981696d8d0987a7e3d121530fade8ddfb))


### BREAKING CHANGES

* remove autofac dependency from idempotency

## [7.1.4](https://github.com/informatievlaanderen/command-handling/compare/v7.1.3...v7.1.4) (2022-12-31)


### Bug Fixes

* bump dependencies ([0aa755c](https://github.com/informatievlaanderen/command-handling/commit/0aa755c083177ee82dde755e812f28c097cb5ff6))

## [7.1.3](https://github.com/informatievlaanderen/command-handling/compare/v7.1.2...v7.1.3) (2022-12-28)


### Bug Fixes

* use bumped event-handling ([a028e3a](https://github.com/informatievlaanderen/command-handling/commit/a028e3a69aca832fa928ec2df0210330aba7233d))

## [7.1.2](https://github.com/informatievlaanderen/command-handling/compare/v7.1.1...v7.1.2) (2022-12-28)


### Bug Fixes

* add IdemPotency.Microsoft nuget package ([3aef7b1](https://github.com/informatievlaanderen/command-handling/commit/3aef7b1d41e7be702e0fc7c795d11a8715e7dce3))

## [7.1.1](https://github.com/informatievlaanderen/command-handling/compare/v7.1.0...v7.1.1) (2022-12-28)


### Bug Fixes

* add Microsoft.Extensions.DependencyInjection ([81c2c31](https://github.com/informatievlaanderen/command-handling/commit/81c2c31d36971a3efdbe2dd2b92eda5756459044))
* update build.fsx ([fbee21c](https://github.com/informatievlaanderen/command-handling/commit/fbee21c70fa3f4057f342bde1e2b0ea718790108))

# [7.1.0](https://github.com/informatievlaanderen/command-handling/compare/v7.0.0...v7.1.0) (2022-12-08)


### Bug Fixes

* add braces ([fe1ca32](https://github.com/informatievlaanderen/command-handling/commit/fe1ca3251853fc3273d6d95d8d1ce93f67a69bcd))
* add nuget to dependabot ([65e8d8e](https://github.com/informatievlaanderen/command-handling/commit/65e8d8e9e3336756940f93b69c22a04d7c2ad372))
* don't throw general exceptions ([999b4b7](https://github.com/informatievlaanderen/command-handling/commit/999b4b7a3ee6472f89a6a34475935809dd6e8523))
* empty methods ([c7a4c68](https://github.com/informatievlaanderen/command-handling/commit/c7a4c68505fee7ff5d30fad7fb8656262883739e))
* field to property ([8b20aef](https://github.com/informatievlaanderen/command-handling/commit/8b20aefb78cd070d8cced86d81a51c7f968a26c9))
* implement Dispose correctly ([0059703](https://github.com/informatievlaanderen/command-handling/commit/00597036eb243e00290943909f5a792f3bcc8562))
* make type parameter contravariant ([90eec7b](https://github.com/informatievlaanderen/command-handling/commit/90eec7bb95c6a04926effd9bed3a759d283c4fc5))
* message.Metadata is optional ([ac4133e](https://github.com/informatievlaanderen/command-handling/commit/ac4133e8104bf464d655a47bde7a2f9f49343ecf))
* seal utility classes ([b053873](https://github.com/informatievlaanderen/command-handling/commit/b053873b6b33faf4e631f451e82c5bde5de24fff))
* use .Add return value ([0a994f4](https://github.com/informatievlaanderen/command-handling/commit/0a994f421b140515180952f2a400ffa110391cd5))
* use immutable dictionary ([e241854](https://github.com/informatievlaanderen/command-handling/commit/e24185499303f99eb8997c397215b6126da18af0))
* use VBR_SONAR_TOKEN ([f7158df](https://github.com/informatievlaanderen/command-handling/commit/f7158dfc9f0870ce8e5d7dcb03ce47b844066cdc))


### Features

* add extension to execute idempotency not just on applicationbuilder ([64b1d4e](https://github.com/informatievlaanderen/command-handling/commit/64b1d4efc986be774c00bdb59f38d8cde548e828))

# [7.0.0](https://github.com/informatievlaanderen/command-handling/compare/v6.2.2...v7.0.0) (2022-08-24)


### Bug Fixes

* restore snapshot use version instead of position ([6ee104b](https://github.com/informatievlaanderen/command-handling/commit/6ee104beed5009b197cc914b3c1023e685caef94))


### BREAKING CHANGES

* - SnapshotStrategyContext no longer has SnapshotPosition
- SnapShotInfo Position renamed to StreamVersion

## [6.2.2](https://github.com/informatievlaanderen/command-handling/compare/v6.2.1...v6.2.2) (2022-07-26)


### Bug Fixes

* make DomainException Serialiable-conformant ([0fa3131](https://github.com/informatievlaanderen/command-handling/commit/0fa3131651804fe7d2bccb126c91ed47c9d72ffa))

## [6.2.1](https://github.com/informatievlaanderen/command-handling/compare/v6.2.0...v6.2.1) (2022-06-17)


### Bug Fixes

* read snapshot correct column, add repository ctor ([f1cd6fe](https://github.com/informatievlaanderen/command-handling/commit/f1cd6fea4cf33a9be107d71bdf872de810798601))

# [6.2.0](https://github.com/informatievlaanderen/command-handling/compare/v6.1.0...v6.2.0) (2022-06-17)


### Features

* add snapshot support in separate table ([428a511](https://github.com/informatievlaanderen/command-handling/commit/428a5111033fe4f350d146896a6b32fa4272b3d1))

# [6.1.0](https://github.com/informatievlaanderen/command-handling/compare/v6.0.2...v6.1.0) (2022-05-11)


### Features

* add AfterEventsCountStrategy, AfterEventTypeStrategy ([b3ed9b4](https://github.com/informatievlaanderen/command-handling/commit/b3ed9b470e0ba50714c543e86bc6f9c828e4464a))
* add NoSnapshotStrategy ([56f4314](https://github.com/informatievlaanderen/command-handling/commit/56f431408439126eed6ab38650dc3ba1fa0e5fdd))

## [6.0.2](https://github.com/informatievlaanderen/command-handling/compare/v6.0.1...v6.0.2) (2022-04-29)


### Bug Fixes

* run sonar end when release version != none ([a6ba01c](https://github.com/informatievlaanderen/command-handling/commit/a6ba01caaed325f58dd51eb7f53e5d8406fea7e9))

## [6.0.1](https://github.com/informatievlaanderen/command-handling/compare/v6.0.0...v6.0.1) (2022-04-27)


### Bug Fixes

* redirect sonar to /dev/null ([64a1567](https://github.com/informatievlaanderen/command-handling/commit/64a1567e8a4f1d245a1b223ba476b3963d5b2d2c))

# [6.0.0](https://github.com/informatievlaanderen/command-handling/compare/v5.3.0...v6.0.0) (2022-03-25)


### Features

* move to dotnet 6.0.3 ([1657b9c](https://github.com/informatievlaanderen/command-handling/commit/1657b9c4ec085e718969ed0a734b3543fe6fd0ad))


### BREAKING CHANGES

* move to dotnet 6.0.3

# [5.3.0](https://github.com/informatievlaanderen/command-handling/compare/v5.2.1...v5.3.0) (2022-03-01)


### Bug Fixes

* eventwithmetadata ctor ([8da0617](https://github.com/informatievlaanderen/command-handling/commit/8da06176e06282381faed251d4f8cb207df83d68))
* put last commit in comments (didn't build) ([556d775](https://github.com/informatievlaanderen/command-handling/commit/556d775d72312a19f55d5ccf6c3d51db82c40226))


### Features

* support Throws<TException> ([06711cc](https://github.com/informatievlaanderen/command-handling/commit/06711ccdabe8c512689781548e311edbc09bb978))

## [5.2.1](https://github.com/informatievlaanderen/command-handling/compare/v5.2.0...v5.2.1) (2021-05-28)


### Bug Fixes

* move to 5.0.6 ([6b6b3f0](https://github.com/informatievlaanderen/command-handling/commit/6b6b3f06fabaf2cc08874a6a30f75b8fc1b11c27))

# [5.2.0](https://github.com/informatievlaanderen/command-handling/compare/v5.1.6...v5.2.0) (2021-04-23)


### Features

* add streamversion to snapshot context + fix interval strategy ([#87](https://github.com/informatievlaanderen/command-handling/issues/87)) ([9e2d5ff](https://github.com/informatievlaanderen/command-handling/commit/9e2d5ff33a0679376514d5371b26de2039dcdfa6))

## [5.1.6](https://github.com/informatievlaanderen/command-handling/compare/v5.1.5...v5.1.6) (2021-04-12)


### Bug Fixes

* revert commits changing EventSpecTestRunner ([b46fd6f](https://github.com/informatievlaanderen/command-handling/commit/b46fd6f6b8d769049903324dd158b0d2e6d4c811))

## [5.1.5](https://github.com/informatievlaanderen/command-handling/compare/v5.1.4...v5.1.5) (2021-04-11)


### Bug Fixes

* filter position conflicted by stream filter ([08b4a20](https://github.com/informatievlaanderen/command-handling/commit/08b4a20cc21864ed7fa11c6b426bfac13b8f989d))

## [5.1.4](https://github.com/informatievlaanderen/command-handling/compare/v5.1.3...v5.1.4) (2021-04-09)


### Bug Fixes

* interval strategy now doesn't create snapshot on start ([03cbabf](https://github.com/informatievlaanderen/command-handling/commit/03cbabf51eaa1a398def4c19a2e3df260088a34e))

## [5.1.3](https://github.com/informatievlaanderen/command-handling/compare/v5.1.2...v5.1.3) (2021-04-09)


### Bug Fixes

* testing aggregate 'Then' with identifier filters out the specific aggregate ([01d79f7](https://github.com/informatievlaanderen/command-handling/commit/01d79f7a0a6319fc3dac7bcd6652e33541e87771))

## [5.1.2](https://github.com/informatievlaanderen/command-handling/compare/v5.1.1...v5.1.2) (2021-03-17)


### Bug Fixes

* change type name for snapshot messages ([ac64fd0](https://github.com/informatievlaanderen/command-handling/commit/ac64fd041b2d659445a728ecbde6b58ae93eb5c5))

## [5.1.1](https://github.com/informatievlaanderen/command-handling/compare/v5.1.0...v5.1.1) (2021-03-16)


### Bug Fixes

* use strategy to determine snapshotting ([1223c23](https://github.com/informatievlaanderen/command-handling/commit/1223c23f4e655be5028fbb1a5616103c68ae61bc))

# [5.1.0](https://github.com/informatievlaanderen/command-handling/compare/v5.0.13...v5.1.0) (2021-03-15)


### Features

* support reading from snapshots ([2b88102](https://github.com/informatievlaanderen/command-handling/commit/2b88102bacef8debe22ce46426425cdbba029a47))
* support writing snapshots ([70f10fb](https://github.com/informatievlaanderen/command-handling/commit/70f10fb7592bb72e6d46631deb83b79d27635dee))

## [5.0.13](https://github.com/informatievlaanderen/command-handling/compare/v5.0.12...v5.0.13) (2021-02-02)


### Bug Fixes

* move to 5.0.2 ([f41f6a0](https://github.com/informatievlaanderen/command-handling/commit/f41f6a097cdbc556487c1107641b792d2a0daa09))

## [5.0.12](https://github.com/informatievlaanderen/command-handling/compare/v5.0.11...v5.0.12) (2020-12-18)


### Bug Fixes

* move to 5.0.1 ([0cc2206](https://github.com/informatievlaanderen/command-handling/commit/0cc2206e2d5d0f37dc26925de91f541b98329aa8))

## [5.0.11](https://github.com/informatievlaanderen/command-handling/compare/v5.0.10...v5.0.11) (2020-11-19)


### Bug Fixes

* update eventhandling reference ([4950ddc](https://github.com/informatievlaanderen/command-handling/commit/4950ddcb894e0071757274590c01f263a3c3bceb))

## [5.0.10](https://github.com/informatievlaanderen/command-handling/compare/v5.0.9...v5.0.10) (2020-11-18)


### Bug Fixes

* remove set-env usage in gh-actions ([a4d00e2](https://github.com/informatievlaanderen/command-handling/commit/a4d00e22b23f198411950510cbe919cfa30ce455))

## [5.0.9](https://github.com/informatievlaanderen/command-handling/compare/v5.0.8...v5.0.9) (2020-09-21)


### Bug Fixes

* move to 3.1.8 ([fad7bb4](https://github.com/informatievlaanderen/command-handling/commit/fad7bb42c4ead7865313e8dc37471677e5e836ed))

## [5.0.8](https://github.com/informatievlaanderen/command-handling/compare/v5.0.7...v5.0.8) (2020-07-18)


### Bug Fixes

* move to 3.1.6 ([8cced90](https://github.com/informatievlaanderen/command-handling/commit/8cced9048b8176c1345e2d68f1e49f797f8a4465))

## [5.0.7](https://github.com/informatievlaanderen/command-handling/compare/v5.0.6...v5.0.7) (2020-07-02)


### Bug Fixes

* update streamstore ([c837df6](https://github.com/informatievlaanderen/command-handling/commit/c837df62ee139756e938987e6f1b8de298b1b9f8))

## [5.0.6](https://github.com/informatievlaanderen/command-handling/compare/v5.0.5...v5.0.6) (2020-06-19)


### Bug Fixes

* move to 3.1.5 ([8bab6b3](https://github.com/informatievlaanderen/command-handling/commit/8bab6b3a8eb67d33c4f583fd171f586a1bbc072e))

## [5.0.5](https://github.com/informatievlaanderen/command-handling/compare/v5.0.4...v5.0.5) (2020-05-18)


### Bug Fixes

* move to 3.1.4 ([f4b6099](https://github.com/informatievlaanderen/command-handling/commit/f4b60999fea3dd2bbbdc71e23872d89e2a150c25))

## [5.0.4](https://github.com/informatievlaanderen/command-handling/compare/v5.0.3...v5.0.4) (2020-05-14)


### Bug Fixes

* remove Microsoft 3.1.4 referenties ([b5d717a](https://github.com/informatievlaanderen/command-handling/commit/b5d717acb5b345f9e805f4b699f155d389cbbe9c))

## [5.0.3](https://github.com/informatievlaanderen/command-handling/compare/v5.0.2...v5.0.3) (2020-05-13)


### Bug Fixes

* remove trailing double quote ([55bb1a8](https://github.com/informatievlaanderen/command-handling/commit/55bb1a8eb34a628ee66b59b6775d163de3347fcb))

## [5.0.2](https://github.com/informatievlaanderen/command-handling/compare/v5.0.1...v5.0.2) (2020-05-13)


### Bug Fixes

* correct repository check for ci runner ([35f1000](https://github.com/informatievlaanderen/command-handling/commit/35f10005b52a0b9c1fdbaaa24c663097ab1e0000))
* move to GH-actions ([cd454bc](https://github.com/informatievlaanderen/command-handling/commit/cd454bc805f7d66631e168924eb23539a4fa08f9))

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
