using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // populate 4x4 table with icons in random order
            AssignIconsToSquares();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // firstClicked points to first Label control
        // that the player clicks but it will be null
        // if the player hasn't clicked a label yet
        Label firstClicked = null;

        // secondClicked points to the second Label control the the player clicks
        Label secondClicked = null;

        // Create Random object to choose random icons for the squares
        Random random = new Random();

        // Since the font is set to Webdings - each letter is a unique icon
        // each icon repeats twice 
        List<string> icons = new List<string>()
        {
            "j", "j", "O", "O","K", "K", "h", "h",
            "w", "w", "/", "/","d", "d", "u", "u",
        };

        float timeCount = 0.0f;
        

        private void AssignIconsToSquares()
        {
            // There are 16 labels in our table with total 16 icons
            // an icon is pulled at random from list and added to each label
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        // Every label click even is handled by this even handler
        //  "sender" - this label was clicked
        private void label_Click(object sender, EventArgs e)
        {
            timer2.Start();

            // the timer is only on after two non-matching icons have been clicked
            // ignore any clicks if the timer is running
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // If the clicked label is black, the player clicked
                // an icon that's already been revealed --
                // ignore the click
                if (clickedLabel.ForeColor == Color.White)
                    return;

                // If firstClicked is null, this is the first icon in the pair that the player clicked
                // set firstClicked label color to black and return
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.White;

                    return;
                }

                // if the times isnt running and firstClicked isnt null
                // then this is the secondClicked - should set it to be visible
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.White;

                SoundPlayer correctSound = new SoundPlayer(@"c:\Windows\Media\Speech Off.wav");
                correctSound.Play();


                //Check to see if user has won
                CheckForWinner();

                // if icons match - keep the visible
                // reset firstClicked and secondClicked to continue revealing pairs
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;

                    SoundPlayer wrongSound = new SoundPlayer(@"c:\Windows\Media\Speech On.wav");
                    wrongSound.Play();

                    return;
                }

                // player clicked two different icons, the timer will start and hide the icons
                timer1.Start();
            }
        }

        // This timer is started when player clicks two icons that dont match
        // it counts 3/4 of a second and turns itself off hiding both icons
        private void timer1_Tick(object sender, EventArgs e)
        {
            // stop the timer
            timer1.Stop();

            // Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // reset both clicked icons to reset the clicks
            firstClicked = null;
            secondClicked = null;
        }

        // Check if player wins but comparing foreground color to backgroundcolor
        private void CheckForWinner()
        {
            // go though all labels checking to see if icons are matched
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            // if the loop didnt return, it didnt find unmatched icons
            // user has won
            timer2.Stop();

            SoundPlayer winSound = new SoundPlayer(@"c:\Windows\Media\tada.wav");
            winSound.Play();

            MessageBox.Show("Well Done!");
            Close();
        }

        // add timer counter in seconds to see how fast user completes the game
        private void timer2_Tick(object sender, EventArgs e)
        {
            timeLabel.Text = "0.0";

            timeCount = timeCount + 0.1f;
            timeLabel.Text = timeCount.ToString("0.00");
        }
    }
}
