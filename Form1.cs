using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using KJT.Architecture.FiniteStateMachine;
using TestFSM.ObjectModel;

namespace TestFSM {
    public partial class Form1 : Form {
        public Form1() {
            this.InitializeComponent();
        }


        private void loadSTTsButton_Click(object sender, EventArgs e) {
            FSM_STT mySTTCD = new FSM_STT("CDPLAYER", "TestFSM.ObjectModel", 
                                       "Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            STT_State beginState = mySTTCD.addState("Begin");
            STT_State playState = mySTTCD.addState("Playing");
            STT_State pauseState = mySTTCD.addState("Paused");
            STT_State stoppedState = mySTTCD.addState("Stopped");

            // Transitions

            beginState.addTransition("startPlaying", playState);
            beginState.addTransition("stop", stoppedState);
            playState.addTransition("pause", pauseState);
            playState.addTransition("stop", stoppedState);
            pauseState.addTransition("startPlaying", playState);
            pauseState.addTransition("stop", stoppedState);
            stoppedState.addTransition("startPlaying", playState);
            mySTTCD.setInitialState(beginState);

            Debug.WriteLine("FSMSTT create complete " + mySTTCD.getRefClassName());

            // ACTOR
    
            FSM_STT mySTTACTOR = new FSM_STT("ACTOR", "TestFSM.ObjectModel",
                                            "Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            STT_State inWingsState = mySTTACTOR.addState("InWings");
            STT_State onStageState = mySTTACTOR.addState("OnStage");
            STT_State driedState = mySTTACTOR.addState("Dried");
            STT_State bowingState = mySTTACTOR.addState("Bowing");
            STT_State endedState = mySTTACTOR.addState("Ended");
            mySTTACTOR.setInitialState(inWingsState);
            mySTTACTOR.setDeleteWhenEndStateReached();
            mySTTACTOR.setTaskModel(taskAllocation.taskPerInstance);

            // Transitions

            inWingsState.addTransition("receiveCue", onStageState);
            onStageState.addTransition("endOfScene", inWingsState);
            onStageState.addTransition("forgetLine", driedState);
            driedState.addTransition("getPrompt", onStageState);
            driedState.addTransition("endOfScene", inWingsState);
            inWingsState.addTransition("endOfPLay", bowingState);
            bowingState.addTransition("applauseStopped", endedState);
            bowingState.addTransition("applauseStopped", endedState);

            Debug.WriteLine("Form1 - FSMSTT create complete " + mySTTACTOR.getRefClassName());

            this.initialiseSTTsListBox(); // list of STTs loaded in memory

        }

        private void initialiseEventComboBox(FSM_STT mySTT) {
            // need handle on events in the list
            this.eventListComboBox.Items.Clear();
            this.eventListComboBox.Text = "";
            foreach(string item in mySTT.getEventsList()) {
                this.eventListComboBox.Items.Add(item);
            }
        }

        private void initialiseSTTsListBox() {
            // need handle on events in the list
            foreach(FSM_STT item in FSM_STT.getInstanceList().Values) {

                string refClassName = item.getRefClassName();
                this.listOfSTTsListBox.Items.Add(refClassName);
                this.listBox1.Items.Add(refClassName);
                this.listBox2.Items.Add(refClassName);
            }
        }


        private void listOfSTTsListBox_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                FSM_STT mySTT = FSM_STT.findByRefClassName(this.listOfSTTsListBox.SelectedItem.ToString());
                this.initialiseEventComboBox(mySTT); //  populate dropdown list of events in UI
                                                     // using the selected STT

                // we should also populate FSM list as well ?? these would be what we send the events to
                this.initialiseInstancesListBox(mySTT.getRefClassName());
            } catch(Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        private void initialiseInstancesListBox(string v) {
            this.listOfInstancesListBox.Items.Clear();
            foreach(FSM fsm in FSM.getInstanceList().Values) {
                if(fsm.getSTT().getRefClassName() == v) {
                    this.listOfInstancesListBox.Items.Add(fsm.getFSMName());
                }
            }
        }

        private void createOMInstancesButton_Click(object sender, EventArgs e) {
            //create 3 CDPLayers and 4 ACTORS

            try {
                FSM_STT mySTTCD = FSM_STT.findByRefClassName("CDPLAYER");
                new CDPLAYER("cdplayer1", mySTTCD, FSMType.synch);
                new CDPLAYER("cdplayer2", mySTTCD, FSMType.synch);
                CDPLAYER c3 = new CDPLAYER("cdplayer3", mySTTCD, FSMType.synch);
                FSM_Event newEv = new FSM_Event(this, CDPLAYER.Events.startPlaying, c3.getFSM());
                CDPLAYER.postEvent(newEv);

                FSM_STT mySTTACTOR = FSM_STT.findByRefClassName("ACTOR");
                ACTOR a1 = new ACTOR("actor1", mySTTACTOR, FSMType.asynch);
                ACTOR a2 = new ACTOR("actor2", mySTTACTOR, FSMType.asynch);
                ACTOR a3 = new ACTOR("actor3", mySTTACTOR, FSMType.asynch);
                ACTOR a4 = new ACTOR("actor4", mySTTACTOR, FSMType.asynch);

                
            } catch(Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        private void sendEventButton_Click(object sender, EventArgs e) {
            // take the instance from the instances list box and send it the event from the
            // events list box ....
            string eventName = this.eventListComboBox.SelectedItem.ToString();
            string targetFSMName = this.listOfInstancesListBox.SelectedItem.ToString();
            FSM targetFSM = FSM.findByFSMName(targetFSMName);
            if(targetFSM is ASYNCH_FSM aFSM) {
                aFSM.setCallBackUIDelegate(this.updateStateTextBox);
                FSM.postEvent(this, eventName, targetFSM);
            } else {
                FSM.postEvent(this, eventName, targetFSM);
                this.updateStateTextBox(targetFSM);
            }
        }

        public void updateStateTextBox(FSM targetFSM) {

            this.selectedInstanceStateTextBox.Text = targetFSM.getCurrentState().getStateName();
        }

        private void listOfInstancesListBox_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                string selectedInstanceName = this.listOfInstancesListBox.SelectedItem.ToString();
                FSM selectedFSM = FSM.findByFSMName(this.listOfInstancesListBox.SelectedItem.ToString());
                this.selectedInstanceStateTextBox.Text = selectedFSM.getCurrentState().getStateName();
            } catch(Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }
        
        private void writeCodeToFileButton_Click(object sender, EventArgs e) {
            // get the STTs from the selections list
            // looop over the selection as necessary ... 

            foreach(object sel in this.listBox2.SelectedItems) {
                FSM_STT mySTT = FSM_STT.findByRefClassName(sel.ToString());
                FSM_CodeBuilder.writeCodeToFile(this.textBox1.Text, mySTT);
            }

        }

        private void ignoreExistingCodeCheckBox_CheckedChanged(object sender, EventArgs e) {
            // set the value ...
            FSM_CodeBuilder.setOnlyGenerateMissingCode(!this.ignoreExistingCodeCheckBox.Checked);
        }

        private void changeOnEntryCodeButton_Click(object sender, EventArgs e) {
            // register a different method
            FSM_CodeBuilder.setWriteOnEntryCode(this.WriteDifferentOnEntryMethodCode);
        }

        private void WriteDifferentOnEntryMethodCode(FSM_STT theSTT, StringBuilder retVal, STT_State state) {
            retVal.Append("      // Method for Entry XXXX \n\n");
            if( state.getOnEntryAnnotation() != null) {
                retVal.Append("      // Implements ").Append(state.getOnEntryAnnotation()).Append(" \n\n");
            }
            retVal.Append("      public void ").Append(state.getStateName());
            retVal.Append("__onEntry(FSM_Event evt)\n");
            retVal.Append("      {\n");
            retVal.Append("         Debug.WriteLine( \"");
            retVal.Append(theSTT.getRefClassName()).Append(".").Append(state.getStateName());
            retVal.Append("__onEntry() Executing in response to event \" + evt.getEventName());\n");
            if(theSTT.getDeleteWhenEndStateReached() && state.getIsFinalState()) {
                retVal.Append("         //  Delete references as this is an end state and the STT demands it\n");
                retVal.Append("         this.derefenceFSM();\n");
            }
            retVal.Append("      }\n\n");
        }

        private static void WriteDifferentClassCode(FSM_STT theSTT, StringBuilder retVal) {

            // TODO want code gen to provide a switch on the type sync/async passed in as a parameter
            // to the constructor.  Maybe a second constructor for initialising with an event  ?

            string className = theSTT.getRefClassName();
            retVal.Append("\n\n   // ADD CLASS XXXX\n");
            retVal.Append("   public class ").Append(className).Append("\n");
            retVal.Append("   {\n\n");
            string instNameName = className.ToLower() + "Name";
            retVal.Append("      protected string ").Append(instNameName).Append(";\n");
            retVal.Append("      protected FSM fsm;\n\n");
            retVal.Append("      public ").Append(className).Append("( string ")
                .Append(instNameName).Append(", FSM_STT stt, FSMType fsmType )\n");
            retVal.Append("      {\n");
            retVal.Append("         this.").Append(instNameName).Append(" = ").Append(instNameName).Append(";\n");
            retVal.Append("         this.fsm = FSM.createFSM(this.").Append(instNameName).
                                    Append(", stt, this, fsmType);\n");
            retVal.Append("         this.fsm.setInitialState();\n");
            retVal.Append("      }\n\n");
            retVal.Append("      public FSM getFSM()\n");
            retVal.Append("      {\n");
            retVal.Append("         return this.fsm;\n");
            retVal.Append("      }\n\n");
            retVal.Append("      public STT_State getCurrentState()\n");
            retVal.Append("      {\n");
            retVal.Append("         return this.fsm.getCurrentState();\n");
            retVal.Append("      }\n\n");
            retVal.Append("      // Use this in the body of your StateName__XXXX() methods when\n");
            retVal.Append("      // you want to 'delete the FSM' ( remember you can't delete in c# )\n");
            retVal.Append("      // So instead we remove references to tidy stuff up.\n");
            retVal.Append("      protected void dereferenceFSM()\n");
            retVal.Append("      {\n");
            retVal.Append("         FSM.removeFromInstanceList(this.fsm);\n");
            retVal.Append("         this.fsm = null;\n");
            retVal.Append("      }\n\n");
            retVal.Append("      // Processes an event.  Passes it on to the FSM\n");
            retVal.Append("      public void takeEvent( FSM_Event evt)\n");
            retVal.Append("      {\n");
            retVal.Append("         this.fsm.takeEvent( evt);\n");
            retVal.Append("      }\n\n");

        }

        
        private void button1_Click(object sender, EventArgs e) {
            // pick a directory to generate the code in
            //
            using(FolderBrowserDialog fbd = new FolderBrowserDialog()) {

                fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
                
                DialogResult result = fbd.ShowDialog();

                if(result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath)) {
                    this.textBox1.Text = fbd.SelectedPath;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            // load from smcat file.
            using(OpenFileDialog fbd = new OpenFileDialog()) {

                fbd.InitialDirectory = ".\\";
                DialogResult result = fbd.ShowDialog();

                if(result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName)) {
                    FSM_FileImporter.SMCatImport( fbd.FileName);
                }

                //then call fileImporter
                
            }
        }
    }
}