using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ClipboardHistory
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Prüfe, ob sich der Inhalt der Zwischenablage geändert hat
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();
                if (listBox.Items.Count == 0 || !listBox.Items[0].ToString().StartsWith(clipboardText))
                {
                    // Füge den neuen Inhalt der Zwischenablage der ListBox hinzu
                    listBox.Items.Insert(0, clipboardText + ": " + DateTime.Now.ToString("hh:mm:ss"));
                }
                
            }
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e, bool delete)
        {
            // Kopiere den ausgewählten Eintrag wieder in die Zwischenablage
            if (listBox.SelectedItem != null)
            {
                string selectedItem = listBox.SelectedItem.ToString();
                string clipboardText = selectedItem.Split(':')[0];
                Clipboard.SetText(clipboardText);

                if (delete)
                {
                    int index = listBox.SelectedIndex;
                    listBox.Items.RemoveAt(index);
                    Clipboard.Clear();
                }
            } else
            {
                MessageBox.Show("Kein Item ausgewählt!");
            }
        }

        private void copyButton_Click_1(object sender, RoutedEventArgs e)
        {
            // Kopiere den ausgewählten Eintrag wieder in die Zwischenablage
            ListBox_SelectionChanged(sender, null, false);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            ListBox_SelectionChanged(sender, null, true);
        }
    }
}
