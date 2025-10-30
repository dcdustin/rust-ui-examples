## Rust UI Tests

Dead simple plugin for reading JSON files into CUI. This plugin is intended to run on a development server, and assumes you are the only player. It will display/destroy UI components to 

### Install

- Put UiTests.cs in carbon/plugins directory
- Move _UiTests_ to Carbon data directoy after plugin
- Add JSON files and reload plugin as much as your little heart desires.


### Usage

- type `uitests` into F1 console to see a list of available UI files from the data/UiTests directory. Reload plugin after adding new JSON files.
- type `uitests.done` to destroy UI.
- Basic JSON layout provided in `ui-example.json`
