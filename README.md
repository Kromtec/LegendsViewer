[![Bay12Forum Thread](https://img.shields.io/badge/Bay12-Forum-blue.svg)](http://www.bay12forums.com/smf/index.php?topic=154617.0)
[![DFFD Download](https://img.shields.io/badge/DFFD-Download-blue.svg)](http://dffd.bay12games.com/file.php?id=11455)
[![Build status](https://ci.appveyor.com/api/projects/status/2dcxapcp3tium23l?svg=true)](https://ci.appveyor.com/project/Kromtec/legends-viewer)

# Legends Viewer

Recreates Legends Mode from exported data. Browser like navigation, including tabs, allows to easily view other people/places/entities by just clicking names in the event logs or search lists, CTRL+Click opens a new tab. 

## Getting the data from Dwarf Fortress:
> :high_brightness: If you are already playing a fort or adventure on the save you want to use, you need to:
> * Make a **copy** of your save!!!
> * Abandon or retire your fortress/adventure on the copy
> * Enter DF legends mode on the copy

**Recommended:** (requires [DFHack](https://github.com/DFHack/dfhack/releases) which is usually included in the Lazy Newb Pack)

1. Enter DF legends mode on the save you want to export 
1. Switch to the DFHack window
1. Enter the command `exportlegends info` and press enter
   1. this command generates all files you need and an additional file with the ending legends_plus.xml
   1. the legends_plus.xml file contains a lot of information that is missing if you export via the old way

**Or use the old way:**

1. Enter Legends Mode. 
1. Export the XML `(x)`
1. Export Map/Gen Information `(p)`
1. (Optional) 
    1. Export Additional Detailed Maps `(d)` 
    1.  Archive/Compress files
	
## Opening the data with Legends Viewer.
Select the XML or Archive first by pressing the "..." button next to the XML / Archive text box.
Legends Viewer will attempt to automatically find the remaining files.
If the viewer was unable to locate a file it will indicate which files by turning the text box red. 
You will then have to manually select those files by pressing the "..." button for each file.

The files needed are (which will be dumped in the root Dwarf Fortress directory)
* `savename-datestamp`-legends.xml
* `savename-datestamp`-legends_plus.xml (Optional)
* `savename-datestamp`-world_history.txt
* `savename-datestamp`-world_sites_and_pops.txt
* `savename-datestamp`-world_map.bmp (or any other Map Image file)
	
## Using Legends Viewer
### General
CTRL+Click on any links, lists, or buttons opens pages in a new tab.

### Searching
* Each tab has various options for sorting and filtering.
* Every tab has the sub tab "Events" letting you filter events from event logs.
* The "Filtered Events" Sort is used in conjunction with the Events Tab which allows for some custom sorting.
* "Filter All Unimportant Warfare" in the Wars Tab removes unnotable battles and pillagings from battle lists in all pages.
* Some lists in pages can be loaded for searching by clicking "[Load]" next to the list heading.

### Map
* Click & Drag moves the map.
* `Mouse Wheel` to zoom in / out.
* Civs & Sites can be toggled with options above the minimap.
* `CTRL + Mouse Wheel` changes year of the map by 1 year.
* `SHIFT + Mouse Wheel` changes year of the map by 10 years.
* `CTRL + SHIFT + Mouse Wheel` changes year of the map by 100 years.

### Advanced Search
* Searching Sub Lists 
  * Example: Searching historical figure's kills. 
  * If you want to see historical figures with kills > 10 just leave the sub options blank. 
  * If you want to see historical figures that killed an elf then you can select Kills > Race > Equals > Elf. 
  * If you want to see who killed the most elves you do the same with Order Criteria.
* Selecting
  * Selecting is used to search and return results from sub lists. 
  * So if you want to search through a specific historical figure's kills, you Historical Figures > Kills, 
    then in the Select Criteria select Name > Equals > "Urist McDwarf". 
  * That will return all of Urist McDwarf's kills, then you can add Search/Order Criteria to look through them more.
* Guided Searches
  * When selecting values to search by it will generate all the available options. 
  * So if you search for Dwarves and then add a criteria for caste it will show you all the castes of dwarves in the 
    drop down box.
* Order (Min/Max/Sum/Avg)
  * You can do Min/Max/Sum/Avg when ordering by sub sub options.
  * So if you want to find historical figures that participated in the most bloody battles you can order by 
    Battles > Deaths > Sum > Descending. 
  * These options can be a little slow sometimes, limiting the results to sort can speed it up.
		
## Things to note: :blue_book:
* __XML dump is incomplete!__ Anywhere that there are details left out I noted with "UNKNOWN" however if you see any  
  "INVALID" then its a bug that needs to be fixed. If you export the data with DFHacks' `exportlegends info` a lot of 
  additional data is available and you will get less "UNKNOWN" information.
* Duplicate Historical Figures / Entities in summary log indicate that relations for duplicates may not be setup 
  properly you will have to check History/Site files to view these relations.
* Bigger worlds will perform slower. Some pages with a lot of activity can take a few seconds to load. Be careful in 
  the Era tab with events and era length, all events are filtered out by default. Selecting too many events and a 
  lengthy era can result in GBs of memory being used and can take some time to generate the page.
* Requires [.NET 4](http://www.microsoft.com/downloads/en/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992&displaylang=en)    and [IE9+](http://windows.microsoft.com/en-us/internet-explorer/download-ie)

## Archive Support 
* Tested with (zip/7zip/rar)
* Must have these files in the archive
  * XML file "-legends.xml"
  * history file ending with "-world_history.txt"
  * sites and populations file ending with "-world_sites_and_pops.txt"
  * Map Image files (bmp/png/jpg/jpeg)
  * (Optional) DFHack exported XML-Plus file "-legends_plus.xml"
* Any other files in the archive will be ignored without error.	
	
## Troubleshooting
* If images or charts aren't showing up in pages or navigating pages doesn't work try updating Internet Explorer to a newer 
  version. IE9 and up should work.
* If you encouter errors while loading files, first try restarting Dwarf Fortress and re-export all the files. 
  If it still happens after that you can send me your files or world gen parameters and I will look into it.
