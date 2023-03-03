NeonImageSorter by NeonLightning

Just Something i'm making to sort my images to learn some c# and make my interpolation software happy.



summary by chatgpt

This is a C# program for a tool called NeonImageSorter. It allows users to select and sort image files, and then move or copy them to a specified folder with a chosen naming convention. Here is a breakdown of the major components of the program:


The program is built using Windows Forms and consists of a single form called MainForm.

The user interface includes a ListView control (Photos) for displaying the selected image files, as well as several buttons for adding, removing, and sorting the images.

The Add button allows the user to select either a folder or individual files to add to the Photos list. If the user selects a folder, all image files within that folder will be added.

The Clear button removes all items from the Photos list.

The Remove button deletes the selected items from the Photos list and, optionally, from disk if the Shift key is held down when the button is clicked.

The Up and Down buttons allow the user to move the selected items up or down in the list.

The Move button moves or copies the selected image files to a specified folder with a naming convention. If the Shift key is held down, the files are copied; otherwise, they are moved. If a file with the same name already exists in the output folder, the 

program will add a number to the end of the filename to make it unique.

The PreviewBox displays a preview image that is stored as a resource in the program.

Overall, the program provides a simple interface for organizing and renaming image files.



Screenshots

![Screenshot Of Use](https://user-images.githubusercontent.com/2992888/222262239-a7ef3911-ca8c-4333-9b8d-9ff1c5d495e2.png)

![settings](https://user-images.githubusercontent.com/2992888/222249359-19c8b68b-fa33-4cc5-a6f5-e363282b4a8e.png)
