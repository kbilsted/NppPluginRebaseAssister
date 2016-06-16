# RebaseAssister plugin for Notepad++

What does the plugin do? Basically it auto-selects the first word of every non-comment line in an interactive rebase file..

Specifically it features

  * Auto-selection of commands, making it super fast to change those `pick` instructions during interactive rebasing.
  * Syntax highlighting

Using Notepad++ during interactive rebasing offers additional benefits 

  * Shortcuts for moving lines up/down (Ctrl+Shift+Up/Down)
  * Spell checking (using the great `DSpellCheck` plugin)
  * Verbose backup - you always have a backup of the guids of the rebase needed for salvaging deleted commits.
  
  
# Installation
This plugin is too new to have been added to the notepad++ pluginmanager - that takes a couple of months to get in place. So manual installation only.

  * Instruct Git / hg to use Notepad++
    * `git config --global core.editor "'C:/Program Files (x86)/Notepad++/notepad++.exe' -multiInst -notabbar -nosession"`
  * Download a binary release and stick the `RebaseAssister.dll` inside the `c:\program files (x86)\notepad++\plugins\` folder. Alternatively, download the source and build it - the build will copy the dll to the notepad folder.



# Misc
This plugin has been created using the `Notepad++ pluginpack for .Net` https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net/releases

Also check out my blog at http://firstclassthoughts.co.uk/
 
