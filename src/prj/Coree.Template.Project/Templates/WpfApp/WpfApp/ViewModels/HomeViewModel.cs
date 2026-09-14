using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
#if( Sqlitedb )
using Microsoft.Data.Sqlite;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        public readonly WindowViewModel WindowViewModel;
        public readonly NavbarViewModel NavbarViewModel;

        private string titleSource = "Source";

        public string TitleSource
        {
            get { return titleSource; }
            set { SetProperty(ref titleSource, value) ; WindowViewModel.TitleSource = titleSource; SetHelperCondition(value); SetHint(value); }
        }

        private void SetHelperCondition(string TitleSource)
        {
            if (TitleSource == "Source")
            {
                Helper = "HintAssist.HelperText";
            }
            else
            {
                Helper = "Press shift+enter to reset and open pane";
            }
        }

        private void SetHint(string TitleSource)
        {
            if (!String.IsNullOrEmpty(TitleSource))
            {
                Hint = String.Concat(TitleSource.Reverse());
            }
            else
            {
                Hint = String.Empty;
            }
        }

        [ObservableProperty]
        private string hint = "HintAssist.Hint";

        [ObservableProperty]
        private string helper = "HintAssist.HelperText";

        [ObservableProperty]
        private string buttonText = "Button";

        [RelayCommand]
        private void SetTitleSourceString()
        {
            TitleSource = "Source";
            Hint = "HintAssist.Hint";
            Helper = "HintAssist.HelperText";

            #if( Sqlitedb )
             //The connection needs be keeps open for memory databases.
            //const string connectionString = "Data Source=InMemorySample;Mode=Memory;Cache=Shared";

            using (var connection = new SqliteConnection("Data Source=demo.db"))
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

            using (var connection = new SqliteConnection("Data Source=demo.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
SELECT [Text]
FROM TestTable
WHERE id = $id
";
                command.Parameters.AddWithValue("$id", "3");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader.GetString(0);
                        ButtonText = $"Sqlite: {name}!";
                    }
                }
            }
#else
            ButtonText = "Button Pressed";
#endif
        }

        [RelayCommand]
        public void EnterKeyDown()
        {
            TitleSource = "Source";
            Hint = "HintAssist.Hint";
            Helper = "HintAssist.HelperText";
            NavbarViewModel.IsOpen = !NavbarViewModel.IsOpen;
        }


        public HomeViewModel()
        {
            WindowViewModel = App.Services!.GetRequiredService<WindowViewModel>();
            NavbarViewModel = App.Services!.GetRequiredService<NavbarViewModel>();
        }
    }
}