using System;
using System.Collections.Generic;
using System.Text;
using KJT.Architecture.FiniteStateMachine;

namespace TestFSM.ObjectModel {
    class RECORDPLAYER {


        // Typedef for States Name strings.  These are inner classes so that
        // instead of writing state names like "receiveCue" in code (and possibly making
        // an error) - instead write ACTOR.States.receiveCue and intellisense will help 

        public class States {
            public const string PlayerOff = "Player Off";
            public const string Stopped = "Stopped";
            public const string IsItDone = "^IsItDone";
            public const string Playing = "Playing";
            public const string Paused = "Paused";
            public const string initial = "initial";
        }

        // Typedef for Event Name strings

        public class Events {
            public const string play = "play";
            public const string stop = "stop";
            public const string pause = "pause";
            public const string powerOn = "powerOn";
            public const string powerOff = "powerOff";
            public const string yes = "yes";
            public const string no = "no";
        }

        protected string recordplayerName;
        protected FSM fsm;

        public RECORDPLAYER(string recordplayerName, FSM_STT stt, FSMType fsmType) {
            this.recordplayerName = recordplayerName;
            this.fsm = FSM.createFSM(this.recordplayerName, stt, this, fsmType);
            this.fsm.setInitialState();
        }

        public FSM getFSM() {
            return this.fsm;
        }

        public STT_State getCurrentState() {
            return this.fsm.getCurrentState();
        }

        // Use this in the body of your StateName__XXX() methods if you want to 
        // 'delete the FSM' - remember there is no delet in c# - so we will just
        // null all the references to tidy stuff up.  Default code generation
        // uses this in onEntry() for final states IF deleteWhenEndStateReached is set 
        protected void dereferenceFSM() {
            FSM.removeFromInstanceList(this.fsm);
            this.fsm = null;
        }

        // Processes an event.  Passes it on to the FSM
        public static void postEvent(FSM_Event evt) {
            FSM.postEvent(evt);
        }

    }
}
