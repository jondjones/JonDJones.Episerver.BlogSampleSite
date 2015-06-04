EPiServer.Packaging, EPiServer.Packaging.UI


Installation
============

Starting from release 3.1, EPiServer Packaging and Packaging UI components have been converted to
standard nuget packages. This means that content files and assemblies will be added to the project.
As a part of the conversion the previously installed add-on packages needs to be removed.

There are several project configuration steps performed as a part of the package installation; these
are documented below. If any of the configuration steps fail during the installation a message box
will be displayed describing the issue.


Remove add-on assemblies
------------------------

This step removes Nuget.Core.dll, EPiServer.Packaging.dll, EPiServer.Packaging.UI.dll and
EPiServer.Packaging.CmdApi.dll from the modulesbin folder. If you have reconfigured the probing path
for your site, then these assemblies will be removed from the configured location.

If you receive an error during this step, then the process installing the nuget package does not
have permission to delete the files. In this case, a user with the appropriate permissions will need
to delete EPiServer.Packaging.dll, EPiServer.Packaging.UI.dll, EPiServer.Packaging.CmdApi.dll and
Nuget.Core.dll from the modulesbin folder manually.


Update packages.config
----------------------

This step removes the EPiServer.Packaging and EPiServer.Packaging.UI package entries from the
packages.config file in order to unregister them from the add-on system.

If you receive an error during this step, then the process installing the nuget package does not
have permission to read or write the file. In this case, a user with the appropriate permissions
will need to remove the EPiServer.Packaging and EPiServer.Packaging.UI package entries manually. By
default, the packages.config file is located inside the modules folder in appdata.


Remove add-on folders
---------------------

This step removes the EPiServer.Packaging and EPiServer.Packaging.UI folders and their contents from
the modules folder.

If you receive an error during this step, then the process installing the nuget package does not
have permission to delete the folders or their contents. In this case, a user with the appropriate
permissions will need to remove the EPiServer.Packaging and EPiServer.Packaging.UI folders manually.
By default, these folders are located inside the modules folder in appdata.


Moving protected modules inside the web application
---------------------------------------------------

Starting from release 7.6 of EPiServer.Framework, the disk location of EPiServer AddOns is being
streamlined. This means that from now on, all the EPiServer developed site AddOns will be placed
inside the modules folder located under site root. All the protected modules will be placed under
modules/_protected/. EPiServer.CMS.UI and EPiServer.Packaging.UI has already been moved to this
location as a part of the conversion to nuget packages.

EPiServer encourges partners and third-party site owner add-on to place their protected addons
inside the modules/_protected location.

In order to facilitate the move of currently installed protected add-ons, the EPiServer.Packaging
provides a powershell cmdlet which can be run inside Visual Studio's Package Manager Console. This
cmdlet will locate the current protected modules folder and move all its contents to the
modules/_protected folder inside the web application.

You can open the Package Manager Console (Tools --> Library/Nuget Package Manager --> Package
Manager Console) and execute Move-EPiServerProtectedModules. This will copy all the contents from
the current protected modules folder path to modules/_protected and then delete the old folder. This
will also update the web.config and related configuration files.

Before running this cmdlet, please make sure you have read/write/delete permissions to the
[appData]\Modules folder and the modules/_protected folder inside the web application.
