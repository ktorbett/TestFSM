﻿using System.Diagnostics;
using TestFSM.FiniteStateMachine;

namespace TestFSM.ObjectModel {

    public class ACTOR {

        protected string actorName;
        protected FSM fsm;

        public ACTOR(string actorName, FSM_STT stt) {
            this.actorName = actorName;
            this.fsm = new FSM(this.actorName, stt, this);
            this.fsm.initialise();
        }

        public STT_State getCurrentState() {
            return this.fsm.getCurrentState();
        }

        // Use this in the body of your StateName__onEntry() methods for the end states
        // of the FSM ( the ones with no exit transitions ) and want to 'delete the FSM'
        // and references to tidy stuff up.
        protected void dereferenceFSM() {
            FSM.removeFromInstanceList(this.fsm);
            this.fsm = null;
        }

        // Implementation of State InWings

        // Method for Entry 

        public void InWings__onEntry(FSM_Event evt) {
            Debug.WriteLine("ACTOR.InWings__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void InWings__onExit(FSM_Event evt) {
            Debug.WriteLine("ACTOR.InWings__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition InWings__receiveCue__OnStage

        public void InWings__receiveCue(FSM_Event evt) {
            Debug.WriteLine("ACTOR.InWings__receiveCue() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition InWings__receiveCue__OnStage

        public bool InWings__receiveCueGuard(FSM_Event evt) {
            Debug.WriteLine("ACTOR.InWings__receiveCueGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition InWings__endOfPLay__Bowing

        public void InWings__endOfPLay(FSM_Event evt) {
            Debug.WriteLine("ACTOR.InWings__endOfPLay() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition InWings__endOfPLay__Bowing

        public bool InWings__endOfPLayGuard(FSM_Event evt) {
            Debug.WriteLine("ACTOR.InWings__endOfPLayGuard() allowing event " + evt.getEventName());
            return true;
        }


        // Implementation of State OnStage

        // Method for Entry 

        public void OnStage__onEntry(FSM_Event evt) {
            Debug.WriteLine("ACTOR.OnStage__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void OnStage__onExit(FSM_Event evt) {
            Debug.WriteLine("ACTOR.OnStage__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition OnStage__endOfScene__InWings

        public void OnStage__endOfScene(FSM_Event evt) {
            Debug.WriteLine("ACTOR.OnStage__endOfScene() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition OnStage__endOfScene__InWings

        public bool OnStage__endOfSceneGuard(FSM_Event evt) {
            Debug.WriteLine("ACTOR.OnStage__endOfSceneGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition OnStage__forgetLine__Dried

        public void OnStage__forgetLine(FSM_Event evt) {
            Debug.WriteLine("ACTOR.OnStage__forgetLine() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition OnStage__forgetLine__Dried

        public bool OnStage__forgetLineGuard(FSM_Event evt) {
            Debug.WriteLine("ACTOR.OnStage__forgetLineGuard() allowing event " + evt.getEventName());
            return true;
        }


        // Implementation of State Dried

        // Method for Entry 

        public void Dried__onEntry(FSM_Event evt) {
            Debug.WriteLine("ACTOR.Dried__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void Dried__onExit(FSM_Event evt) {
            Debug.WriteLine("ACTOR.Dried__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition Dried__getPrompt__OnStage

        public void Dried__getPrompt(FSM_Event evt) {
            Debug.WriteLine("ACTOR.Dried__getPrompt() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Dried__getPrompt__OnStage

        public bool Dried__getPromptGuard(FSM_Event evt) {
            Debug.WriteLine("ACTOR.Dried__getPromptGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition Dried__endOfScene__InWings

        public void Dried__endOfScene(FSM_Event evt) {
            Debug.WriteLine("ACTOR.Dried__endOfScene() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Dried__endOfScene__InWings

        public bool Dried__endOfSceneGuard(FSM_Event evt) {
            Debug.WriteLine("ACTOR.Dried__endOfSceneGuard() allowing event " + evt.getEventName());
            return true;
        }


        // Implementation of State Bowing

        // Method for Entry 

        public void Bowing__onEntry(FSM_Event evt) {
            Debug.WriteLine("ACTOR.Bowing__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void Bowing__onExit(FSM_Event evt) {
            Debug.WriteLine("ACTOR.Bowing__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition Bowing__applauseStopped__InWings

        public void Bowing__applauseStopped(FSM_Event evt) {
            Debug.WriteLine("ACTOR.Bowing__applauseStopped() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Bowing__applauseStopped__InWings

        public bool Bowing__applauseStoppedGuard(FSM_Event evt) {
            Debug.WriteLine("ACTOR.Bowing__applauseStoppedGuard() allowing event " + evt.getEventName());
            return true;
        }


    }


}