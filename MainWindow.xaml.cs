using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;

namespace BasketballStatistics {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        struct NBALeaguePlayers {
            public string Player;
            public string Team;
            public bool Rookie;
            public double Rating;
            public int Games;
            public double MinutesPG;
            public double PointsPG;
            public double ReboundsPG;
            public double AssistsPG;
            public double ShotPercentage;
            public double FreethrowPercentage;
            public double AwardFinder;
        }//end struct


        NBALeaguePlayers[] globalLeagueData;
        int globalRecord;

       

        public MainWindow() {
            InitializeComponent();
        }//END MAIN
        #region EVENTS


        private void muiOpen_Click(object sender, RoutedEventArgs e) {


            //create the open file dialog object
            OpenFileDialog ofd = new OpenFileDialog();

            //open the dialog and wait for the user to make a selection
            bool fileSelected = (bool)ofd.ShowDialog();

            if (fileSelected == true) {
                //get record count
                globalRecord = RecordCount(ofd.FileName, true);
                //set slider to match
                sldRecord.Maximum = globalRecord - 1;
                //load data from csv & return array of person
                globalLeagueData = ProcessLeagueData(ofd.FileName, globalRecord, true);
                //update form with 1st persons data
                UpdateLeague(0);
                ChangeAwards((int)sldAwards.Value);
                UpdateAwards((int)sldAwards.Value);
            }//end if
        }//end event


        private void sldRecord_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

            //check if no data has been loaded exit if this is the case
            if (globalLeagueData == null) {
                sldRecord.Value = 0;
                return;
            }//end if

            //create an int to snap slider value to
            int sliderInt = (int)sldRecord.Value;

            //update label showin the cerrently selceted record
            lblPlayerIndex.Content = (sliderInt + 1).ToString();
            //update form
            UpdateLeague(sliderInt);
            
        }//end event


        private void btnBegining_Click(object sender, RoutedEventArgs e) {
            sldRecord.Value = sldRecord.Minimum;
            

        }//end event

        private void btnprevious_Click(object sender, RoutedEventArgs e) {
            sldRecord.Value--;
            

        }//end event

        private void btnnext_Click(object sender, RoutedEventArgs e) {
            sldRecord.Value++;
           

        }//end event

        private void btnEnd_Click(object sender, RoutedEventArgs e) {
            sldRecord.Value = sldRecord.Maximum;
            

        }//end event

        private void sldAwards_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

            int awardCount = 0;
            

            //check if no data has been loaded exit if this is the case
            if (globalLeagueData == null) {
                sldAwards.Value = 0;
                return;
            }//end if


            string[] NBAAwards = { "Prius Award", "Gas Guzzler Award", "Foul Target Award", "Overachiever Award", "UnderAchiever Award", "On The Fence Award(s)", "Bang For Your Buck Award", "Gordon Gekko Award", "Charlie Brown Awards", "Tiger Uppercut Awards" };

                int NumAwards = NBAAwards.GetLength(0) - 1;
                sldAwards.Maximum = NumAwards;
                awardCount = (int)sldAwards.Value;
                
                ChangeAwards(awardCount);
                UpdateAwards(awardCount);

        }//end event
        #endregion

        #region METHODS/FUNCTION

        int RecordCount(string filepath, bool skipHeader) {

            int record = 0;

            StreamReader infile = new StreamReader(filepath);

            if (skipHeader) {
                infile.ReadLine();
            }//end if

            while (infile.EndOfStream == false) {
                infile.ReadLine();
                record++;
            }//end while

            infile.Close();

            return record;
        }//end function

        NBALeaguePlayers[] ProcessLeagueData(string filepath, int recordCount, bool skipHeader) {

            NBALeaguePlayers[] leagueData = new NBALeaguePlayers[recordCount];
            int currentCounter = 0;
            StreamReader infile = new StreamReader(filepath);

            if (skipHeader) {
                infile.ReadLine();
            }//end if



            while (infile.EndOfStream == false && currentCounter < recordCount) {

                string record = infile.ReadLine();

                string[] field = record.Split(",");




                leagueData[currentCounter].Player = field[0];
                leagueData[currentCounter].Team = field[1];
                leagueData[currentCounter].Rookie = Convert.ToBoolean(int.Parse(field[2]));
                leagueData[currentCounter].Rating = LeagueParse(field[3]);
                leagueData[currentCounter].Games = (int) LeagueParse(field[4]);
                leagueData[currentCounter].MinutesPG = LeagueParse(field[5]);
                leagueData[currentCounter].PointsPG = LeagueParse(field[6]);
                leagueData[currentCounter].ReboundsPG = LeagueParse(field[7]);
                leagueData[currentCounter].AssistsPG = LeagueParse(field[8]);
                leagueData[currentCounter].ShotPercentage = LeagueParse(field[9]);
                leagueData[currentCounter].FreethrowPercentage = LeagueParse(field[10]);
                
                currentCounter++;
            }//end while

            infile.Close();

            return leagueData;

        }//end function

        double LeagueParse(string leagueData) {
            double parsedValue = 0;
            bool parsed = false;

            parsed = double.TryParse((leagueData), out parsedValue);

            return parsedValue;
        }//end function

        void UpdateLeague(int PlayerIndex) {
            //grab a person form the global array
            NBALeaguePlayers currentPlayer = globalLeagueData[PlayerIndex];
            //update textboxes on the form
            txtPlayer.Text = currentPlayer.Player;
            txtTeam.Text = currentPlayer.Team.ToString();
            txtRookie.Text =  currentPlayer.Rookie.ToString();
            txtRating.Text = currentPlayer.Rating.ToString();
            txtGames.Text = currentPlayer.Games.ToString();
            txtMinutes.Text = currentPlayer.MinutesPG.ToString();
            txtPoints.Text = currentPlayer.PointsPG.ToString();
            txtRebounds.Text = currentPlayer.ReboundsPG.ToString();
            txtAssists.Text = currentPlayer.AssistsPG.ToString();
            txtShotPercentage.Text = currentPlayer.ShotPercentage.ToString();
            txtFreethrowPercentage.Text = currentPlayer.FreethrowPercentage.ToString();




        }//end function

        private void ChangeAwards (int AwardIndex) {
            string[] NBAAwards = { "Prius Award", "Gas Guzzler Award", "Foul Target Award", "Overachiever Award", "UnderAchiever Award", "On The Fence Award(s)", "Bang For Your Buck Award", "Gordon Gekko Award", "Charlie Brown Awards", "Tiger Uppercut Awards" };

            txtAwards.Text = NBAAwards[AwardIndex];
        }//end function
        private void UpdateAwards(int value) {
            if (sldAwards.Value == 0 ) {
                #region PriusAward
                ChangeAwards(0);
                NBALeaguePlayers[] nbaPlayer = PriusBubbleSort(globalLeagueData);
                Array.Reverse(nbaPlayer);
                NBALeaguePlayers winner = nbaPlayer[0];
                txtAwardWinner.Text = winner.Player;
                txtPlayer.Text = winner.Player;
                txtTeam.Text = winner.Team;
                txtRookie.Text = winner.Rookie.ToString();
                txtRating.Text = winner.Rating.ToString();
                txtGames.Text = winner.Games.ToString();
                txtMinutes.Text = winner.MinutesPG.ToString();
                txtPoints.Text = winner.PointsPG.ToString();
                txtRebounds.Text = winner.ReboundsPG.ToString();
                txtAssists.Text = winner.AssistsPG.ToString();
                txtShotPercentage.Text = winner.ShotPercentage.ToString();
                txtFreethrowPercentage.Text = winner.FreethrowPercentage.ToString();
                #endregion 
            }else if (sldAwards.Value == 1) {
                #region Gas Guzzler
                ChangeAwards(1);
                NBALeaguePlayers[] nbaPlayer = GasGuzzlerBubbleSort(globalLeagueData);
                NBALeaguePlayers winner = nbaPlayer[0];
                txtAwardWinner.Text = winner.Player;
                txtPlayer.Text = winner.Player;
                txtTeam.Text = winner.Team;
                txtRookie.Text = winner.Rookie.ToString();
                txtRating.Text = winner.Rating.ToString();
                txtGames.Text = winner.Games.ToString();
                txtMinutes.Text = winner.MinutesPG.ToString();
                txtPoints.Text = winner.PointsPG.ToString();
                txtRebounds.Text = winner.ReboundsPG.ToString();
                txtAssists.Text = winner.AssistsPG.ToString();
                txtShotPercentage.Text = winner.ShotPercentage.ToString();
                txtFreethrowPercentage.Text = winner.FreethrowPercentage.ToString();
                #endregion 
            }else if (sldAwards.Value == 2) {
                #region Foul Target
                ChangeAwards(2);

                #endregion 
            }else if (sldAwards.Value == 3) {
                #region Overachiever
                ChangeAwards(3);

                #endregion 
            }else if (sldAwards.Value == 4) {
                #region UnderAchiever
                ChangeAwards(4);

                #endregion 
            }else if (sldAwards.Value == 5) {
                #region On The Fence
                ChangeAwards(5);

                #endregion 
            }else if (sldAwards.Value == 6) {
                #region Bang For Your Buck
                ChangeAwards(6);

                #endregion 
            }else if (sldAwards.Value == 7) {
                #region Gordon Gekko
                ChangeAwards(7);

                #endregion 
            }else if (sldAwards.Value == 8) {
                #region Charlie Brown
                ChangeAwards(8);

                #endregion 
            }else if (sldAwards.Value == 9) {
                #region Tiger Uppercut Awards
                ChangeAwards(9);

                #endregion 
            }//end if
        }//end function
        #region Awards
        NBALeaguePlayers[] PriusBubbleSort(NBALeaguePlayers[] SeasonStat) {
     
            
            bool swappedValue = true;
            int maxPosition = globalLeagueData.GetLength(0) - 1;
     
            while (swappedValue == true) {
     
                swappedValue = false;
     
     
     
                for (int index = 0; index < maxPosition; index += 1) {
                    
     
                    if (SeasonStat[index].MinutesPG != 0 && SeasonStat[index+1].MinutesPG != 0) {
     
                        SeasonStat[index].AwardFinder = (SeasonStat[index].PointsPG * SeasonStat[index].Games) / (SeasonStat[index].MinutesPG * SeasonStat[index].Games);
                        SeasonStat[index+1].AwardFinder = (SeasonStat[index+1].PointsPG * SeasonStat[index + 1].Games) / (SeasonStat[index+1].MinutesPG * SeasonStat[index+1].Games);
     
                    }//end if
                }//end for
                maxPosition = maxPosition - 1;
            }//end while
            return Awardee(SeasonStat);
        }//end function
        private NBALeaguePlayers[] Awardee(NBALeaguePlayers[] Awardee) {

            bool swappedValue = true;
            int maxPosition = globalLeagueData.GetLength(0) - 1;


            while (swappedValue == true) {

                swappedValue = false;

                for (int index = 0; index < maxPosition; index++) {

                    if (Awardee[index + 1].AwardFinder < Awardee[index].AwardFinder) {
                        //STORED VALUE ON THE RIGHT
                        NBALeaguePlayers placeHolder = Awardee[index + 1];

                        //REPLACE VALUE ON THE RIGHT WITH VALUE ON THE LEFT
                        Awardee[index + 1] = Awardee[index];

                        //PUT STORED VALUE INTO ARRAY SLOT ON THE LEFT
                        Awardee[index] = placeHolder;

                        swappedValue = true; 
                    }//end if
                }//end for
                maxPosition = maxPosition - 1;
            }//end while

            return Awardee;
        }//end function

        NBALeaguePlayers[] GasGuzzlerBubbleSort(NBALeaguePlayers[] SeasonStat) {


            bool swappedValue = true;
            int maxPosition = globalLeagueData.GetLength(0) - 1;

            while (swappedValue == true) {

                swappedValue = false;



                for (int index = 0; index < maxPosition; index += 1) {


                    if (SeasonStat[index].MinutesPG != 0 && SeasonStat[index + 1].MinutesPG != 0) {

                        SeasonStat[index].AwardFinder = (SeasonStat[index].PointsPG * SeasonStat[index].Games) / (SeasonStat[index].MinutesPG * SeasonStat[index].Games);
                        SeasonStat[index + 1].AwardFinder = (SeasonStat[index + 1].PointsPG * SeasonStat[index + 1].Games) / (SeasonStat[index + 1].MinutesPG * SeasonStat[index + 1].Games);

                    }//end if
                }//end for
                maxPosition = maxPosition - 1;
            }//end while
            return Awardee(SeasonStat);
        }//end function
        #endregion 
        #endregion


    }//end class
}//end namespace
