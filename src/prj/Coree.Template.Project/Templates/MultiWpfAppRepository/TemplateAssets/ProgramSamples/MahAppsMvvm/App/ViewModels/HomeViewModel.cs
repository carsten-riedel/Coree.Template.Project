using System;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Data.Sqlite;

namespace __SourceName__.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private const string ConnectionString = "Data Source=demo.db";
        private string titleSource = "Source";

        public WindowViewModel WindowViewModel { get; }

        public NavbarViewModel NavbarViewModel { get; }

        public string TitleSource
        {
            get => titleSource;
            set
            {
                SetProperty(ref titleSource, value);
                WindowViewModel.TitleSource = titleSource;
                SetHelperCondition(value);
                SetHint(value);
            }
        }

        [ObservableProperty]
        public partial string Hint { get; set; } = "HintAssist.Hint";

        [ObservableProperty]
        public partial string Helper { get; set; } = "HintAssist.HelperText";

        [ObservableProperty]
        public partial string ButtonText { get; set; } = "Button";

        public HomeViewModel(WindowViewModel windowViewModel, NavbarViewModel navbarViewModel)
        {
            ArgumentNullException.ThrowIfNull(windowViewModel);
            ArgumentNullException.ThrowIfNull(navbarViewModel);

            WindowViewModel = windowViewModel;
            NavbarViewModel = navbarViewModel;
        }

        private void SetHelperCondition(string value)
        {
            Helper = value == "Source"
                ? "HintAssist.HelperText"
                : "Press shift+enter to reset and open pane";
        }

        private void SetHint(string value)
        {
            Hint = !String.IsNullOrEmpty(value)
                ? String.Concat(value.Reverse())
                : String.Empty;
        }

        [RelayCommand]
        private void SetTitleSourceString()
        {
            TitleSource = "Source";
            Hint = "HintAssist.Hint";
            Helper = "HintAssist.HelperText";

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
DROP TABLE IF EXISTS [TestTable];

CREATE TABLE IF NOT EXISTS [TestTable] (
  [Id] INTEGER NOT NULL
, [Text] TEXT NOT NULL
, CONSTRAINT [PK_TestTable] PRIMARY KEY ([Id])
);

INSERT OR IGNORE INTO [TestTable] ([Id],[Text]) VALUES (1,'Text1');
INSERT OR IGNORE INTO [TestTable] ([Id],[Text]) VALUES (2,'Text2');
INSERT OR IGNORE INTO [TestTable] ([Id],[Text]) VALUES (3,$TextVar);
";
                command.Parameters.AddWithValue("$TextVar", "ButtonPressed");
                command.ExecuteNonQuery();
            }

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
SELECT [Text]
FROM TestTable
WHERE id = $id
";
                command.Parameters.AddWithValue("$id", "3");

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var name = reader.GetString(0);
                    ButtonText = $"Sqlite: {name}!";
                }
            }
        }

        [RelayCommand]
        public void EnterKeyDown()
        {
            TitleSource = "Source";
            Hint = "HintAssist.Hint";
            Helper = "HintAssist.HelperText";
            NavbarViewModel.IsOpen = !NavbarViewModel.IsOpen;
        }
    }
}
