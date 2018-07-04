using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIMLbot;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Chatbot
{
    public partial class Form1 : Form
    {

        Bot TheBot = new Bot();
        User newUser;
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
       (
           int nLeftRect,     // x-coordinate of upper-left corner
           int nTopRect,      // y-coordinate of upper-left corner
           int nRightRect,    // x-coordinate of lower-right corner
           int nBottomRect,   // y-coordinate of lower-right corner
           int nWidthEllipse, // height of ellipse
           int nHeightEllipse // width of ellipse
       );


        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            this.Opacity = 1;

            timer1.Start();
            vScrollBar1.Hide();

            Bot AI = new Bot(); //new bot

            AI.loadSettings(); // This loads the settings from the config folder

            AI.loadAIMLFromFiles(); // This loads the AIML files from the aiml folder

            AI.isAcceptingUserInput = false; // This swithes off the bot to stop the user entering input while the bot is loading

            User myUser = new User("Username", AI); // This creates a new User called "Username", using the object "AI"'s information.

            AI.isAcceptingUserInput = true; // This swithces the user input back on

            TheBot = AI;
            newUser = myUser;

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 5, 5));

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool TextInserted()     // checks if text is inserted
        {
            if (textBox1.Text == "")
            {
                return false;
            }
            return true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) //Text in textbox changed
        {
            textBox1.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        }

        //setup for chat phases and initliazing relevant variables
        int nMessages = 0;
        int nLines = 0;
        TextBox[] boxie = new TextBox[30];
        Boolean phase1passed = false;
        Boolean phase2passed = false;
        Boolean phase3passed = false;
        Boolean phase4passed = false;
        Boolean phase5passed = false;
        Boolean phase6passed = false;
        Boolean phase7passed = false;
        Boolean finished = false;
        String Emotionlevel;
        String Reflectionlevel;
        String suggestion;
        String professional;


        private void textBox1_KeyDown(object sender, KeyEventArgs e) //sending a message by checking if pressed button is the enter button
        {
            // push message
            if (e.KeyCode == Keys.Enter && TextInserted())
            {
                //Debug.WriteLine(TheBot.DefaultPredicates.grabSetting("name"));
                //Debug.WriteLine(nMessages);
                String input = textBox1.Text.Trim();                  // copy voor printen 
                messageLeft(input);

            }

        }

        private void BotActivate(String input) // The bot activates based on a reaction to the input string
        {
            if (!phase5passed || finished) { 
            Request r = new Request(input, newUser, TheBot);
            Result res = TheBot.Chat(r);

            Debug.WriteLine("response box made");
            String response = "Psychbot: " + res;
            messageRight(response);
            
            }
            //////////
            //phases//
            //////////
            Boolean afterOther = false;

                
            //phase 1
            if (nMessages > 3 && newUser.Predicates.grabSetting("name") == "un-named user" && !phase1passed && !afterOther)
            {
                Debug.WriteLine("phase1");
                messageRight("Psychbot: What is your name?");
                phase1passed = true;
                afterOther = true;
            }

            //phase 2
            if (nMessages > 3 && newUser.Predicates.grabSetting("name") != "un-named user" && !phase2passed && !afterOther)
            {
                Debug.WriteLine("phase2");
                messageRight("Psychbot: hey " + newUser.Predicates.grabSetting("name") + ", how old are you?");
                phase2passed = true;
                afterOther = true;

            }

            //phase 3
            if (nMessages > 10 && newUser.Predicates.grabSetting("name") != "un-named user" && newUser.Predicates.grabSetting("age") != "unknown" && !phase3passed && !afterOther)
            {
                Debug.WriteLine("phase3");
                messageRight("Psychbot: So what is it that you feel like you are struggling with? Think about your answer, the simpler you formulate it, the better I'll be able to help.");
                phase3passed = true;
                afterOther = true;
            }

            //phase 4
            if (nMessages > 12 && phase3passed && !afterOther && !phase4passed)
            {
                Debug.WriteLine("phase4");
                messageRight("Psychbot: Okay I think I understand. I would like to ask you some short questions to better assist you, would that be okay with you?");
                phase4passed = true;
                afterOther = true;
            }

            //phase 5

            if (!afterOther && phase4passed && !phase5passed)
            {
                Debug.WriteLine("phase5");
                messageRight("Psychbot: So first off, on a scale of one to ten, with 1 being purely rational and 10 being very emotional, how emotional of a person do you consider yourself to be?");
                phase5passed = true;
                afterOther = true;
            }

            //phase 6

            if (phase5passed && !afterOther && !phase6passed)
            {
                Debug.WriteLine("phase6");
                Emotionlevel = Regex.Match(input, @"\d+").Value;
                Int32.Parse(Emotionlevel);
                messageRight("Psychbot; " +Emotionlevel +", got it.");
                messageRight("Psychbot: then on a similar scale of 1 to 10, with 1 being very bad and 10 being extremely good. How good do you consider yourself to be at self reflection?");
                phase6passed = true;
                afterOther = true;
            }

            //phase 7 

            if (phase6passed && !afterOther && !phase7passed)
            {
                Debug.WriteLine("phase7");
                Reflectionlevel = Regex.Match(input, @"\d+").Value;
                Int32.Parse(Reflectionlevel);
                messageRight("Psychbot: okay, so " + Reflectionlevel + ", noted.");
                phase7passed = true;
                afterOther = true;

                int level;
                level = Int32.Parse(Reflectionlevel) + 10 - Int32.Parse(Emotionlevel);
                

                Button professionalButton = new Button();
                professionalButton.Location = new Point(7, 102);
                professionalButton.Size = new Size(271, 89);
                professionalButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
                professionalButton.FlatAppearance.BorderSize = 0;
                professionalButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

                if (level < 4)
                {
                    suggestion = "an alternative type of therapy, like movement therapy.";
                    professional = "Ms A. Zarate";
                    professionalButton.Image = global::Chatbot.Properties.Resources.trcontacts_02;
                }
                else if (level < 8)
                {
                    suggestion = "a mindfulness therapy, which is a therapy that focusses on emotional control and relaxation.";
                    professional = "Mr P. THibodeau";
                    professionalButton.Image = global::Chatbot.Properties.Resources.trcontacts_03;
                }
                else if (level < 12)
                {
                    suggestion = "Humanistic therapy, a type of therapy that helps you apply your positive traits in the here and now, to improve the present.";
                    professional = "Dr J. Ols";
                    professionalButton.Image = global::Chatbot.Properties.Resources.trcontacts_07;
                }
                else if (level < 16)
                {
                    suggestion = "Psychoanalytic therapy. In this type of therapy you have intimate discussions with a therapist, where they help you vent and better understand the problems you are dealing with.";
                    professional = "Drs B.H. van Beek";
                    professionalButton.Image = global::Chatbot.Properties.Resources.trcontacts_05;
                }
                else
                {
                    suggestion = "a Cognitive behavioral therapy. This therapy allows for you to gain a deeper insight into what behavior is causing certain issues and helps you in methodically changing that.";
                    professional = "Dr S.G. Lugo";
                    professionalButton.Image = global::Chatbot.Properties.Resources.trcontacts_06;
                }

                messageRight("Psychbot: Thanks for that, based on our conversation, I would suggest " + suggestion 
                    + "For this type of treament, you can contact: " + professional +". I took the liberty to add them to your contact list, in case you want to start there.");
                this.panel6.Controls.Add(professionalButton);
                professionalButton.Click += new System.EventHandler(this.professionalButton_Click);
                finished = true;
            }

        }

        private void professionalButton_Click(object sender, EventArgs e) //clicking of a professional button in the contact list 
        {
            for (int i = 0; i < nMessages; i++)
            {
                boxie[i].Hide();
            }
            textBox3.Text= "Send your first message to " + professional + ":";
            button1.Hide();
            makeTextBox4();
            textBox1.ReadOnly = true;
            panel9.Show();
            panel9.BringToFront();
        }

        private void makeTextBox4() //interactions for the sending of a first message
        {
            TextBox textBox4 = new TextBox();
            textBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            textBox4.ForeColor = System.Drawing.Color.Black;
            panel9.Controls.Add(textBox4);
            textBox4.Location = new System.Drawing.Point(33,100);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(351, 80);
            textBox4.TabIndex = 0;
            textBox4.Text = "";
            textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            textBox4.BringToFront();

            Button buttonmail = new Button();
            buttonmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            buttonmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(101)))), ((int)(((byte)(140)))));
            buttonmail.Location = new System.Drawing.Point(129, 195);
            buttonmail.Name = "buttonmail";
            buttonmail.Size = new System.Drawing.Size(152, 40);
            buttonmail.TabIndex = 1;
            buttonmail.Text = "Send an email";
            buttonmail.UseVisualStyleBackColor = true;
            panel9.Controls.Add(buttonmail);
            buttonmail.Click += new System.EventHandler(this.buttonmail_Click);

        }

        private void buttonmail_Click(object sender, EventArgs e)
        {
            
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Psychbot_MouseHover(object sender, EventArgs e) //hover Psychbot button
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.Psychbot, "Chat function");
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            this.Opacity = 200;
        }

        TextBox[] Settingbox = new TextBox[8];

        private void button2_Click(object sender, EventArgs e) //clicking of the user profile button
        {
            panel5.BringToFront();

            for (int i = 0; i < 4; i++)
            {
                Settingbox[i] = new TextBox();

                Settingbox[i].BackColor = Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(230)))), ((int)(((byte)(236)))));
                Settingbox[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Settingbox[i].ForeColor = System.Drawing.Color.Black;
                Settingbox[i].BorderStyle = BorderStyle.None;
                Settingbox[i].Location = new System.Drawing.Point(50, 50 + 100 * i);
                Settingbox[i].Multiline = true;
                Settingbox[i].Size = new System.Drawing.Size(500, 300);
                Settingbox[i].ReadOnly = true;
                panel5.Controls.Add(Settingbox[i]);
                Settingbox[i].BringToFront();
            }

            Settingbox[0].Text = "name = " + newUser.Predicates.grabSetting("name");
            Settingbox[1].Text = "e-mail = " + newUser.Predicates.grabSetting("email");
            Settingbox[2].Text = "age = " + newUser.Predicates.grabSetting("age");
            Settingbox[3].Text = "location = " + newUser.Predicates.grabSetting("location");

        }

        private void button2_MouseHover(object sender, EventArgs e) //hover button 2; user page
        {
            System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
            ToolTip2.SetToolTip(this.button2, "Your profile");
        }

        private void button3_MouseHover(object sender, EventArgs e)//hover button 3; exit
        {
            System.Windows.Forms.ToolTip ToolTip3 = new System.Windows.Forms.ToolTip();
            ToolTip3.SetToolTip(this.button3, "Exit");
        }

        private void Settings_MouseHover(object sender, EventArgs e) // hover settings
        {
            System.Windows.Forms.ToolTip ToolTip4 = new System.Windows.Forms.ToolTip();
            ToolTip4.SetToolTip(this.Settings, "Settings");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e) //clears textbox when clicked
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e) //clears textbox when clicked
        {
            textBox2.Text = "";
            pictureBox1.Hide();
            textBox2.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) //exits app when button 3 is clicked
        {
            Application.Exit();
        }

        int StartupCounter = 0;

        private void timer1_Tick_1(object sender, EventArgs e) //times start of the app and first message
        {
            StartupCounter++;
            Debug.WriteLine(StartupCounter);

            if (StartupCounter<30)
            {
                //this.Controls.Add(pictureBox2);

            }
            else
            {
                messageRight("Psychbot: Hello, nice to meet you, my name is Psychbot, an AI here to assist you in finding the best expert to help you out.");
                timer1.Stop();
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Psychbot_Click_1(object sender, EventArgs e) //open the chat with Psychbot
        {
            ContactClicked();
            textBox1.ReadOnly = false;
            for(int i=0; i < nMessages; i++)
            {
                boxie[i].Show();
            }
            if (nLines > 10)
            {
                vScrollBar1.Show();
                vScrollBar1.BringToFront();
            }
        }

        private void ContactClicked() //a chatbox is opened because a contact is clicked
        {
            panel3.BringToFront();
            textBox2.BringToFront();
            panel5.SendToBack();
            panel6.BringToFront();
            panel4.BringToFront();
            panel1.BringToFront();
            panel2.BringToFront();
            pictureBox1.BringToFront();
        }

        private void messageRight(String input) //publishes a message on the right side of the screen
        {
            var t = Task.Run(async delegate
            {
                await Task.Delay(1500);
                return;
            });
            t.Wait();

            int tokens = 35;
            int q = 40;

            boxie[nMessages] = new TextBox();
            boxie[nMessages].Text = input;
            boxie[nMessages].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            boxie[nMessages].Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            boxie[nMessages].ForeColor = System.Drawing.Color.Black;
            boxie[nMessages].BorderStyle = BorderStyle.None;
            boxie[nMessages].Multiline = true;
            boxie[nMessages].Location = new System.Drawing.Point(500, 50 + 40 * nLines);

            for(int i=1; i < 7; i++)
            {
                if (input.Length > i * tokens)
                {
                    q += 40;
                    nLines ++;
                }
            }
            
            nLines++;

            boxie[nMessages].Size = new System.Drawing.Size(450, q + 20);
            boxie[nMessages].ReadOnly = true;
            panel3.Controls.Add(boxie[nMessages]);
            boxie[nMessages].BringToFront();
            boxie[nMessages].TextAlign = HorizontalAlignment.Right;

            if (nLines > 10)
            {
                vScrollBar1.Show();
                panel3.Location = new System.Drawing.Point(panel3.Location.X, 77 - 40 * (nLines - 10));
            }

            nMessages++;
            panel1.BringToFront();
        }

        private void messageLeft(String input) //publishes a message on the left
        {

            int tokens = 35;
            int q = 40;

            boxie[nMessages] = new TextBox();
            boxie[nMessages].Text = " You:   " + input;
            boxie[nMessages].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            boxie[nMessages].Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            boxie[nMessages].ForeColor = System.Drawing.Color.Black;
            boxie[nMessages].BorderStyle = BorderStyle.None;
            boxie[nMessages].Location = new System.Drawing.Point(75, 50 + 40 * nLines);

            if (input.Length > 3 * tokens)
            {
                q += 3 * 40;
                nLines += 3;
            }
            else if (input.Length > 2 * tokens)
            {
                q += 2 * 40;
                nLines += 2;
            }
            else if (input.Length > tokens)
            {
                q += 40;
                nLines++;
            }
            nLines++;

            boxie[nMessages].Multiline = true;
            boxie[nMessages].TextAlign = HorizontalAlignment.Left;
            boxie[nMessages].Size = new System.Drawing.Size(450, q + 20);
            boxie[nMessages].ReadOnly = true;
            panel3.Controls.Add(boxie[nMessages]);
            boxie[nMessages].BringToFront();

            if (nLines > 10)
            {
                vScrollBar1.Show();
                panel3.Location = new System.Drawing.Point(panel3.Location.X, 77 - 40 * (nLines - 10));
            }

            textBox1.Clear();
            nMessages++;
            BotActivate(input);
            panel1.BringToFront();
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e) //scrollbar
        {
            float factor = (vScrollBar1.Value / 91f);
            int final = Convert.ToInt32(77 - (40 * (nLines - 10))* factor);
            panel3.Location = new Point(287, final);
        }

        Boolean onboarding = false;
        Boolean onboarding2 = false;

        private void button1_Click(object sender, EventArgs e) //onboarding buttons
        {
            if(onboarding == false)
            {
                textBox3.Text = "This application gathers data automatically through conversation, rather than one having to disclose it seperatly. It does so to help provide the best possible suggestion in the end and is not shared or used for anything else.";
                onboarding = true;
            }
            else
            {
                onboarding2 = true;
                panel9.Hide();
            }
        }

        
    }
}
