# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this package adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.2] - 2023-02-06
### Changed
- Dependencies updated to iet-framework 3.1.3

## [1.2.1] - 2022-08-22
### Added
- Added "Autocomplete page" button to the Authoring toolbar

### Changed
- Updated Tutorial Framework dependency to 3.1.1.
- Raised the minimum required Unity version to 2020.3.

## [1.0.2] - 2022-02-23
### Changed
- Updated Tutorial Framework dependency to 2.2.1.

## [1.0.1] - 2022-02-14
### Added
- Added unit tests back to the package.

### Changed
- Updated Tutorial Framework dependency to 2.2.0.

## [1.0.0] - 2021-07-09

### Changed
- Enable progress tracking by default for Ready-to-Use Tutorials, as is done for the tutorials of Ready-to-Use Tutorial Projects.
- Updated Tutorial Framework dependency to 2.0.0.

### Removed
- **Breaking change**: Removed `CommonTutorialCallbacks` assets. These are moved to Tutorial Framework.

## [1.0.0-pre.5] - 2021-05-19
### Changed
- UI: Cleaned up and restructured the **Tutorials** menu, authoring-related items can be now found under the **Tutorials** > **Authoring** submenu.

### Removed
- Omitted tests from the package.
- Documentation: `*.Tests` namespaces excluded from the Scripting API documentation.

## [1.0.0-pre.4] - 2021-03-10
### Fixed
- Documentation: Fixed all installation instructions to use "-pre.X" postfix.

### Changed
- Updated Tutorial Framework dependency to 2.0.0-pre.4.

## [1.0.0-pre.3] - 2021-03-03
### Changed
- Updated Tutorial Framework dependency to 2.0.0-pre.3.

## [1.0.0-pre.2] - 2021-02-26
### Added
- Added `SelectGameObject` function to `CommonTutorialCallbacks`.
- Documentation: package documentation/manual added.

## [1.0.0-pre.1] - 2020-11-17
### Changed
- Breaking change: assembly and namespace renamed to `Unity.Tutorials.Authoring.Editor`.
- Updated Tutorial Framework dependency to 2.0.0-pre.1.

### Removed
- Removed **Tutorials** > **Export Tutorial** and **Tutorials** > **Export all with default settings** menu items.
Tutorial Exporter was experimental and not supported officially.

## [0.6.6] - 2021-01-15
### Changed
- Updated Tutorial Framework dependency to 1.2.2.

## [0.6.5] - 2020-11-17
### Changed
- Updated Tutorial Framework dependency to 1.2.1.

## [0.6.4] - 2020-11-11
### Added
- Localization support for CJK languages. No actual translations provided yet.

### Changed
- `TutorialStructureExtractor`: use the current Editor language in the default file name, move the menu item under _Localization_ submenu.
- Updated Tutorial Framework dependency to 1.2.0.

## [0.6.3] - 2020-09-22
### Added
- Menu items for creating light and dark tutorial style sheets.
- Functionality and menu items for creating Ready-to-Use Tutorial Project and Ready-to-Use Tutorial.
- Added `TutorialCallbacksHandler` script, one can utilize `CommonTutorialCallbacksHandler` instance or make a copy and modify these for own needs.
- User guide for tutorial authoring can be found from the `UserGuide` folder.

### Changed
- Raised the required Unity version to 2019.4.
- Updated Tutorial Framework dependency to 1.2.0.
- Moved "Run Startup Code" to be the last button of the toolbar, allowing "Preview Masking" to be visible and accessible increasing the window width.
- Unit tests are omitted from the package.

### Fixed
- Cleaned up PO files from dummy test translations.

## [0.6.2] - 2020-08-06
### Changed
- Updated Tutorial Framework dependency to 1.0.2.

## [0.6.1] - 2020-07-22
### Fixed
- Updated Tutorial Framework dependency to 1.0.0 in order to fix compilation.

## [0.6.0] - 2020-07-17
### Added
- Functionality to extract the project's tutorial structure into a text file (_Tutorials_ > _Extract Tutorial Structure of the Project..._).

### Changed
- `GenesisHelperUtils`, `InternalMenuItem`, `TutorialExporter`, and `TutorialExporterWindow` classes are now private instead of public.

## [0.5.1] - 2020-07-06
### Added
- "Select Tutorial" and "Select Page" buttons in the `TutorialWindow`'s toolbar.
- A menu item and a button in `TutorialPageEditor` for creating callback scripts and accompanying assets for handling the various recently added events of the tutorial assets.

### Changed
- Updated to IET Framework 0.5.1.
- The package is now known as Tutorial Authoring Tools instead of IET Authoring Tools.

### Removed
- Removed menu item for creating empty tutorial pages, the other page templates should be used instead.

## [0.5.0] - 2020-06-23
### Changed
- Updated to IET Framework 0.5.0.

## [0.4.0] - 2020-06-02
### Added
- Three new menu items for creating tutorial pages under _Assets_ > _Create_ > _Tutorials_ > _Tutorial Page_.
- _Tutorials_ > _Layout_ > _Window Size_ menu items for setting the size of the main window quickly.
- _Assets_ > _Set Dirty_ menu item for conveniently dirtying tutorial assets while refactoring.

## [0.3.0] - 2020-03-25
### Changed
- Raised the required Unity version to 2019.3.

## [0.2.1] - 2019-11-11
### Added
- Added "Clear InitCodeMarker (Internal)" menu item and functionality.
- Added missing license file for the package.

## [0.2.0] - 2019-10-21
### Changed
- Aligned version with the IET Framework's version.
 
## [0.1.12] - 2019-10-02
### Changed
- Do not save the state of `TutorialWindow` when saving window layout.
### Added
- Menu items for opening the welcome dialog and running the first-launch experience.

## [0.1.11] - 2019-05-15
### Changed
- bumping dependency version

## [0.1.10] - 2019-03-04
### Changed
- bumping dependency version

## [0.1.9] - 2019-02-12
### Changed
- bumping dependency version

## [0.1.8] - 2019-02-04
### Changed
- Change export tutorial to include separate Package Manager manifest file in exported `.unitypackage`
- Change export tutorial to not compile assembly for scripts that need internals

### Added
- Exclude Rider plugin from exported tutorial

## [0.1.7] - 2019-01-24

## [0.1.6] - 2019-01-17

## [0.1.5] - 2019-01-11
### Fixed
- Fix invalid CHANGELOG formatting.

## [0.1.4] - 2018-12-11
### Fixed
- Fixed build script

## [0.1.3] - 2018-12-10
### Added
- Add *Tutorials > Open Tutorials* menu item.
