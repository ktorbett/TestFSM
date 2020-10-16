using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using TestFSM.FiniteStateMachine;
using TestFSM.ObjectModel;

namespace TestFSM {
    public partial class Form1 : Form {
        public Form1() {
            this.InitializeComponent();
        }


        private void loadSTTsButton_Click(object sender, EventArgs e) {
            FSM_STT mySTTCD = new FSM_STT("CDPLAYER", "TestFSM.ObjectModel");

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

            FSM_STT mySTTACTOR = new FSM_STT("ACTOR", "TestFSM.ObjectModel");

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
                this.listOfSTTsListBox.Items.Add(item.refClassName);
                this.listBox1.Items.Add(item.refClassName);
                this.listBox2.Items.Add(item.refClassName);
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
                new CDPLAYER("cdplayer3", mySTTCD, FSMType.synch);

                FSM_STT mySTTACTOR = FSM_STT.findByRefClassName("ACTOR");
                ACTOR a1 = new ACTOR("actor1", mySTTACTOR, FSMType.asynch);
                ACTOR a2 = new ACTOR("actor2", mySTTACTOR, FSMType.asynch);
                ACTOR a3 = new ACTOR("actor3", mySTTACTOR, FSMType.asynch);
                ACTOR a4 = new ACTOR("actor4", mySTTACTOR, FSMType.asynch);

                if(a1.getFSM() is ASYNCH_FSM a1FSM) {
                    Debug.WriteLine("a1 using " + a1FSM.getEventProcessor().caller);
                }
                if(a2.getFSM() is ASYNCH_FSM a2FSM) {
                    Debug.WriteLine("a2 using " + a2FSM.getEventProcessor().caller);
                }
                if(a3.getFSM() is ASYNCH_FSM a3FSM) {
                    Debug.WriteLine("a3 using " + a3FSM.getEventProcessor().caller);
                }
                if(a4.getFSM() is ASYNCH_FSM a4FSM) {
                    Debug.WriteLine("a4 using " + a4FSM.getEventProcessor().caller);
                }
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
                FSM.createAndSendEvent(this, eventName, targetFSM);
            } else {
                FSM.createAndSendEvent(this, eventName, targetFSM);
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
            retVal.Append("      // Method for Entry XXXXXX\n\n");
            retVal.Append("      public void ").Append(state.stateName);
            retVal.Append("__onEntry(FSM_Event evt)\n");
            retVal.Append("      {\n");
            retVal.Append("         Debug.WriteLine( \"");
            retVal.Append(theSTT.refClassName).Append(".").Append(state.stateName);
            retVal.Append("__onEntry() XXXXXXXXXExecuting in response to event \" + evt.getEventName());\n");
            if(theSTT.getDeleteWhenEndStateReached() && state.getIsFinalState()) {
                retVal.Append("         //  XXXXXXXXXXXdelete references as this is an end state and the STT demands it\n");
                retVal.Append("         this.derefenceFSM();\n");
            }
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
    }
}