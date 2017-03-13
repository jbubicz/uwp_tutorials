

----------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------Rules of usege of this of library-----------------------------------------------------


	In "AssemblyLoader" construcor you must specify the path of CONFIG XML file:
	-------------------------------------------------------------------
		Example:
 		new AssemblyLoader( @"\PackagesConfig\LoadingModulesConfig.xml");
	-------------------------------------------------------------------

	In your config file you must specify what libraries you need to load from "AppX" folder.
		NOTE: all libraries of solution are copied to "AppX" folder
	-------------------------------------------------------------------
		Example:
			<?xml version="1.0" encoding="utf-8" ?>
			<!--config file code example-->
			<modules> 
			  <module AssemblyName ="MainMenu"/>
			  <module AssemblyName ="GlobalMenu"/>
			</modules>
    -------------------------------------------------------------------
		NOTE: you may also add new needed attributes, you will have access to these values further via attribute name