﻿/*
    Copyright © 2009 Jesse Graupmann
    All rights reserved.

    Redistribution and use in source and binary forms, with or without 
    modification, are permitted provided that the following conditions 
    are met:

        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright 
          notice, this list of conditions and the following disclaimer 
          in the documentation and/or other materials provided with the 
          distribution.
        * Neither the name of the author nor the names of contributors 
          may be used to endorse or promote products derived from this 
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
    "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
    LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
    FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
    COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
    INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
    BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
    CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
    LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
    ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
    POSSIBILITY OF SUCH DAMAGE.
*/



namespace Artefact.Animation
{
    /// <summary>
    /// Returns true when function is complete.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public delegate bool BoolTickHandler(TickObject obj);

    /// <summary>
    /// Object that subscribes to timer tick for updates. Update function returns true when complete.
    /// </summary>
    public class TickObject
    {
        #region PROPS
        public object Data;
        public BoolTickHandler TickUpdate;
        public bool IsRunning { get; private set; }
        #endregion

        #region Methods
        public void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            ArtefactAnimator.Tick += _Tick;
        }

        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            ArtefactAnimator.Tick -= _Tick;
        }
        #endregion

        #region Tick
        internal void _Tick()
        {
            if (!IsRunning || TickUpdate == null || TickUpdate(this)) Stop();
        }
        #endregion
    }
}