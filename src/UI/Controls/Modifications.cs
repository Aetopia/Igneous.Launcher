using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;

namespace Igneous.Launcher.UI.Controls;

sealed class Modifications : GroupBox
{
    internal readonly HashSet<string> Files;

    internal Modifications(HashSet<string> collection)
    {
        VerticalAlignment = VerticalAlignment.Stretch;
        HorizontalAlignment = HorizontalAlignment.Stretch;

        OpenFileDialog openFileDialog = new()
        {
            CheckFileExists = true,
            CheckPathExists = true,
            DereferenceLinks = true,
            Multiselect = true,
            Filter = "Dynamic-Link Libraries (*.dll)|*.dll"
        };

        Grid grid = new() { Margin = new(4) };
        grid.RowDefinitions.Add(new());
        grid.RowDefinitions.Add(new() { Height = GridLength.Auto });

        Button addButton = new()
        {
            Margin = new(0, 4, 2, 0),
            Content = "➕",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        Button removeButton = new()
        {
            Margin = new(2, 4, 0, 0),
            Content = "➖",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        UniformGrid uniformGrid = new() { Rows = 1 };
        uniformGrid.Children.Add(addButton);
        uniformGrid.Children.Add(removeButton);

        ListBox listBox = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            SelectionMode = SelectionMode.Multiple
        };

        Grid.SetRow(listBox, 0);
        Grid.SetColumn(listBox, 0);
        grid.Children.Add(listBox);

        Grid.SetRow(uniformGrid, 1);
        Grid.SetColumn(uniformGrid, 0);
        grid.Children.Add(uniformGrid);

        Content = grid;

        foreach (var item in Files = collection) listBox.Items.Add(item);

        addButton.Click += (_, _) =>
        {
            if (!(bool)openFileDialog.ShowDialog()) return;
            foreach (var fileName in openFileDialog.FileNames)
            {
                var item = fileName.ToLowerInvariant();
                if (Files.Add(item)) listBox.Items.Add(item);
            }
        };

        removeButton.Click += (_, _) =>
        {
            var selectedItems = listBox.SelectedItems;
            var removedItems = new string[selectedItems.Count];
            selectedItems.CopyTo(removedItems, 0);

            foreach (var item in removedItems) if (Files.Remove(item)) listBox.Items.Remove(item);
        };
    }
}