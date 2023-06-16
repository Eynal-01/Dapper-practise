using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Entities;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDbTransaction commandType;

        public MainWindow()
        {
            InitializeComponent();
            GetAllCaller();

            //var player = GetById(1);
            //player.Score = 50;
            //player.Name = "New Gamer";
            //UPdate(player);
            //GetAllCaller();

            //Insert(new Player
            //{
            //    Name = "John",
            //    Score = 88,
            //    IsStar = false
            //});
            //GetAllCaller();

            //Delete(5);
            //GetAllCaller();

            CallSP(94);

            //myDataGrid.ItemsSource = new List<Player> { player };
        }

        public async void GetAllCaller()
        {
            var players = await GetAllAsync();
            myDataGrid.ItemsSource = players;
        }

        public void CallSP(float score)
        {
            var conn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                var data = connection.Query<Player>("ShowGreaterThan", new {pScore=score}, commandType:CommandType.StoredProcedure);
                myDataGrid.ItemsSource = data;
            }
        }

        public async Task<List<Player>> GetAllAsync()
        {
            List<Player> players = new List<Player>();
            var conn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

            using (var connection = new SqlConnection(conn))
            {
                var data = await connection.QueryAsync<Player>("SELECT Id, PlayerName, Score, IsStar FROM Players");
                players = data.ToList();
            }
            return players;
        }

        public Player GetById(int id)
        {
            var conn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                var player = connection.QueryFirstOrDefault<Player>("SELECT * FROM Players WHERE Id=@PId", new { PId = id });
                return player;
            }
        }

        public void Insert(Player player)
        {
            var conn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                connection.Execute(@"
                    INSERT INTO Players(PlayerName, Score, IsStar)
                    VALUES(@PName, @PScore, @PIsStar)", new { PName = player.Name, PScore = player.Score, PIsStar = player.IsStar });
                MessageBox.Show("Player Added Successfully");
            }
        }

        public void UPdate(Player player)
        {
            var conn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                connection.Execute(@"
                    UPDATE Players
                    SET PlayerName=@PName, Score=@PScore, IsStar=@PIsStar
                    WHERE Id=@PId"
                    , new { PName = player.Name, PScore = player.Score, PIsStar = player.IsStar, PId = player.Id });
            }
        }

        public void Delete(int id)
        {
            var conn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                connection.Execute(@"DELETE FROM Players WHERE Id=@PId", new { PId = id });
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}