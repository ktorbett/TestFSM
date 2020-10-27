﻿using System;
using System.Collections.Generic;
using System.Text;
using KJT.Architecture.FiniteStateMachine;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TestFSM.ObjectModel {
    class RECORDPLAYER {

        // Typedef for States Name strings.  These are inner classes so that
        // instead of writing state names like "receiveCue" in code (and possibly making
        // an error) - instead write ACTOR.States.receiveCue and intellisense will help 

        public class States {
            public const string Player_Off = "Player_Off";
            public const string Stopped = "Stopped";
            public const string Playing = "Playing";
            public const string Paused = "Paused";
            public const string CHOOSE_IsItDone = "CHOOSE_IsItDone";
        }

        // Typedef for Event Name strings

        public class Events {
            public const string powerOn = "powerOn";
            public const string play = "play";
            public const string powerOff = "powerOff";
            public const string stop = "stop";
            public const string pause = "pause";
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

        // Implementation of State Player_Off

        // Method for Entry 

        public void Player_Off__onEntry(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Player_Off__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void Player_Off__onExit(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Player_Off__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition Player_Off__powerOn__Stopped

        public void Player_Off__powerOn(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Player_Off__powerOn() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Player_Off__powerOn__Stopped

        public bool Player_Off__powerOnGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Player_Off__powerOnGuard() allowing event " + evt.getEventName());
            return true;
        }


        // Implementation of State Stopped

        // Method for Entry 

        //TODO: Implement method for : startup_stuff

        public void Stopped__onEntry(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Stopped__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        //TODO: Implement method for : start_turntable

        public void Stopped__onExit(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Stopped__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition Stopped__play__Playing

        public void Stopped__play(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Stopped__play() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Stopped__play__Playing

        //TODO: Implement test for : [up_to_speed]

        public bool Stopped__playGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Stopped__playGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition Stopped__powerOff__Player_Off

        public void Stopped__powerOff(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Stopped__powerOff() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Stopped__powerOff__Player_Off

        public bool Stopped__powerOffGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Stopped__powerOffGuard() allowing event " + evt.getEventName());
            return true;
        }


        // Implementation of State Playing

        // Method for Entry 

        //TODO: Implement method for : start_play

        public void Playing__onEntry(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Playing__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void Playing__onExit(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Playing__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition Playing__stop__Stopped

        //TODO: Implement method for : stop_turntable

        public void Playing__stop(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Playing__stop() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Playing__stop__Stopped

        public bool Playing__stopGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Playing__stopGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition Playing__pause__Paused

        public void Playing__pause(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Playing__pause() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Playing__pause__Paused

        public bool Playing__pauseGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Playing__pauseGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition Playing__powerOff__Player_Off

        public void Playing__powerOff(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Playing__powerOff() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Playing__powerOff__Player_Off

        public bool Playing__powerOffGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Playing__powerOffGuard() allowing event " + evt.getEventName());
            return true;
        }


        // Implementation of State Paused

        // Method for Entry 

        //TODO: Implement method for : raise_needle

        public void Paused__onEntry(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Paused__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void Paused__onExit(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Paused__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition Paused__pause__Playing

        public void Paused__pause(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Paused__pause() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Paused__pause__Playing

        public bool Paused__pauseGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Paused__pauseGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition Paused__stop__Stopped

        //TODO: Implement method for : stop_turntable

        public void Paused__stop(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Paused__stop() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Paused__stop__Stopped

        public bool Paused__stopGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Paused__stopGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition Paused__powerOff__CHOOSE_IsItDone

        public void Paused__powerOff(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Paused__powerOff() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Paused__powerOff__CHOOSE_IsItDone

        public bool Paused__powerOffGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.Paused__powerOffGuard() allowing event " + evt.getEventName());
            return true;
        }


        // Implementation of State CHOOSE_IsItDone

        // Method for Entry 

        public void CHOOSE_IsItDone__onEntry(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.CHOOSE_IsItDone__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void CHOOSE_IsItDone__onExit(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.CHOOSE_IsItDone__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition CHOOSE_IsItDone__yes__Player_Off

        public void CHOOSE_IsItDone__yes(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.CHOOSE_IsItDone__yes() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition CHOOSE_IsItDone__yes__Player_Off

        public bool CHOOSE_IsItDone__yesGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.CHOOSE_IsItDone__yesGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition CHOOSE_IsItDone__no__Stopped

        public void CHOOSE_IsItDone__no(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.CHOOSE_IsItDone__no() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition CHOOSE_IsItDone__no__Stopped

        public bool CHOOSE_IsItDone__noGuard(FSM_Event evt) {
            Debug.WriteLine("RECORDPLAYER.CHOOSE_IsItDone__noGuard() allowing event " + evt.getEventName());
            return true;
        }

    }
}
