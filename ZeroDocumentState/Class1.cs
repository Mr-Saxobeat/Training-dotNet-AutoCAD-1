﻿using Autodesk.Windows;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;

namespace ZeroDocumentState
{
    // Create the command handler for the custom application menu item
    public class MyCommandHandler : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Application.ShowAlertDialog("MyMenuItem has been clicked.");
        }
    }

    class Chapter4
    {
        // Global var for ZeroDocState
        ApplicationMenuItem acApMenuItem = null;

        [CommandMethod("AddZeroDocEvent")]
        public void AddZeroDocEvent()
        {
            // Get the DocumentCollection and register the DocumentDestroyed event
            DocumentCollection acDocMgr = Application.DocumentManager;
            acDocMgr.DocumentDestroyed += new DocumentDestroyedEventHandler(docDestroyed);
        }

        public void docDestroyed(object obj,
                                 DocumentDestroyedEventArgs acDocDesEvtArgs)
        {
            // Determine if the menu item already exists and the number of documents open
            if(Application.DocumentManager.Count == 1 && acApMenuItem == null)
            {
                // Add the event handler to watch for when the application menu is opened
                // AdWindows.dll must be referenced to the project
                ComponentManager.ApplicationMenu.Opening +=
                    new EventHandler<EventArgs>(Applictionmenu_Opening);
            }
        }

        void ApplicationMenu_Opening(object sender, EventArgs e)
        {
            // Check to see if the custom menu item was added previously
            if (acApMenuItem == null)
            {
                // Get the application menu component
                ApplicationMenu acApMenu = ComponentManager.ApplicationMenu;

                // Create a new application menu item
                acApMenuItem = new ApplicationMenuItem();
                acApMenuItem.Text = "MyMenuItem";
                acApMenuItem.CommandHandler = new MyCommandHandler();

                // Append the new menu item
                acApMenu.MenuContent.Items.Add(acApMenuItem);

                // Remove the application menu Opening event handler
                ComponentManager.ApplicationMenu.Opening =
                    new System.EventHandler<System.EventArgs>(ApplicationMenu_Opening);
            }
        }
    }
}
