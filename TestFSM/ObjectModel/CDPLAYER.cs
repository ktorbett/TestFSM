using System.Diagnostics;
using TestFSM.FiniteStateMachine;

namespace TestFSM.ObjectModel {
    internal class CDPLAYER {

        protected string cdplayerName;
        protected FSM fsm;

        public CDPLAYER(string cdplayerName, FSM_STT stt, FSMType fsmType) {
            this.cdplayerName = cdplayerName;
            this.fsm = FSM.createFSM(this.cdplayerName, stt, this, fsmType);
            this.fsm.setInitialState();
        }

        public FSM getFSM() {
            return this.fsm;
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

        // Processes an event.  Passes it on to the FSM
        public void takeEvent(FSM_Event evt) {
            this.fsm.takeEvent(evt);
        }

        // Implementation of State Begin

        // Method for Entry 

        public void Begin__onEntry(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Begin__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void Begin__onExit(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Begin__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition Begin__startPlaying__Playing

        public void Begin__startPlaying(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Begin__startPlaying() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Begin__startPlaying__Playing

        public bool Begin__startPlayingGuard(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Begin__startPlayingGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition Begin__stop__Stopped

        public void Begin__stop(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Begin__stop() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Begin__stop__Stopped

        public bool Begin__stopGuard(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Begin__stopGuard() allowing event " + evt.getEventName());
            return true;
        }


        // Implementation of State Playing

        // Method for Entry 

        public void Playing__onEntry(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Playing__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void Playing__onExit(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Playing__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition Playing__pause__Paused

        public void Playing__pause(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Playing__pause() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Playing__pause__Paused

        public bool Playing__pauseGuard(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Playing__pauseGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition Playing__stop__Stopped

        public void Playing__stop(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Playing__stop() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Playing__stop__Stopped

        public bool Playing__stopGuard(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Playing__stopGuard() allowing event " + evt.getEventName());
            return true;
        }


        // Implementation of State Paused

        // Method for Entry 

        public void Paused__onEntry(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Paused__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void Paused__onExit(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Paused__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition Paused__startPlaying__Playing

        public void Paused__startPlaying(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Paused__startPlaying() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Paused__startPlaying__Playing

        public bool Paused__startPlayingGuard(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Paused__startPlayingGuard() allowing event " + evt.getEventName());
            return true;
        }
        // Method for Transition Paused__stop__Stopped

        public void Paused__stop(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Paused__stop() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Paused__stop__Stopped

        public bool Paused__stopGuard(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Paused__stopGuard() allowing event " + evt.getEventName());
            return true;
        }


        // Implementation of State Stopped

        // Method for Entry 

        public void Stopped__onEntry(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Stopped__onEntry() Executing in response to event " + evt.getEventName());
        }

        // Method for Exit 

        public void Stopped__onExit(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Stopped__onExit() Executing in response to event " + evt.getEventName());
        }

        // Method for Transition Stopped__startPlaying__Playing

        public void Stopped__startPlaying(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Stopped__startPlaying() Executing in response to event " + evt.getEventName());
        }

        // GuardMethod for transition Stopped__startPlaying__Playing

        public bool Stopped__startPlayingGuard(FSM_Event evt) {
            Debug.WriteLine("CDPLAYER.Stopped__startPlayingGuard() allowing event " + evt.getEventName());
            return true;
        }

    }
}
