using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO;

namespace DrugCounterApp;

public partial class MainWindow : Window
{
    private int acetaminophenCount = 0;
    private int oxycotinCount = 0;
    private int ibuprofenCount = 0;

    private string logFile = "DrugIncrementTest.log";

    public MainWindow()
    {
        InitializeComponent();
        CreateLogFile();
    }

    private void CreateLogFile()
    {
        try
        {
            using (var writer = new StreamWriter(logFile, false))
            {
                writer.WriteLine($"{DateTime.Now:MM/dd/yy HH:mm:ss} START");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Failed to create log file: {ex.Message}");
        }
    }

    private void Log(string message)
    {
        try
        {
            File.AppendAllText(logFile, $"{DateTime.Now:MM/dd/yy HH:mm:ss} {message}\n");
        }
        catch (Exception ex)
        {
            ShowError($"Error writing to log file: {ex.Message}");
        }
    }

    private void OnAcetaminophenClicked(object? sender, RoutedEventArgs e)
    {
        int old = acetaminophenCount++;
        AcetaminophenCountText.Text = acetaminophenCount.ToString();
        Log($"Acetaminophen {old} {acetaminophenCount}");
    }

    private void OnOxycotinClicked(object? sender, RoutedEventArgs e)
    {
        int old = oxycotinCount++;
        OxycotinCountText.Text = oxycotinCount.ToString();
        Log($"Oxycotin {old} {oxycotinCount}");
    }

    private void OnIbuprofenClicked(object? sender, RoutedEventArgs e)
    {
        int old = ibuprofenCount++;
        IbuprofenCountText.Text = ibuprofenCount.ToString();
        Log($"Ibuprofen {old} {ibuprofenCount}");
    }

    private void OnResetClicked(object? sender, RoutedEventArgs e)
    {
        Log("RESET");
        Log($"Acetaminophen {acetaminophenCount} 0");
        Log($"Oxycotin {oxycotinCount} 0");
        Log($"Ibuprofen {ibuprofenCount} 0");

        acetaminophenCount = 0;
        oxycotinCount = 0;
        ibuprofenCount = 0;

        AcetaminophenCountText.Text = "0";
        OxycotinCountText.Text = "0";
        IbuprofenCountText.Text = "0";
    }

    private void OnViewLogClicked(object? sender, RoutedEventArgs e)
    {
        try
        {
            string content = File.ReadAllText(logFile);
            var logWindow = new Window
            {
                Title = "Log File",
                Width = 400,
                Height = 300,
                Content = new StackPanel
                {
                    Margin = new Thickness(10),
                    Children =
                    {
                        new ScrollViewer
                        {
                            Content = new TextBlock
                            {
                                Text = content,
                                TextWrapping = Avalonia.Media.TextWrapping.Wrap
                            }
                        },
                        new Button
                        {
                            Content = "Close",
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            Margin = new Thickness(0, 10, 0, 0)
                        }
                    }
                }
            };

            ((Button)((StackPanel)logWindow.Content).Children[1]).Click += (_, _) => logWindow.Close();

            logWindow.ShowDialog(this);
        }
        catch (Exception ex)
        {
            ShowError($"Error reading log file: {ex.Message}");
        }
    }

    private void OnExitClicked(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ShowError(string message)
    {
        var dialog = new Window
        {
            Title = "Error",
            Width = 300,
            Height = 150,
            Content = new StackPanel
            {
                Margin = new Thickness(10),
                Children =
                {
                    new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap },
                    new Button
                    {
                        Content = "OK",
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Margin = new Thickness(0, 10, 0, 0)
                    }
                }
            }
        };

        ((Button)((StackPanel)dialog.Content).Children[1]).Click += (_, _) => dialog.Close();

        dialog.ShowDialog(this);
    }
}
