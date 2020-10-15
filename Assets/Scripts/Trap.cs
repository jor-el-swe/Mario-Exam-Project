using System;
using UnityEngine;

public class Trap : MonoBehaviour
{
        private float trapTimer = 0.0f;
        private bool isTrapActive = true;


        public bool IsTrapActive()
        {
                if (trapTimer >= 2)
                {
                        isTrapActive = true;
                        trapTimer = 0;
                }
                
                if (isTrapActive)
                {
                        isTrapActive = false;
                        return true;
                }

                return false;
        }

        protected void Update()
        { 
                trapTimer += Time.deltaTime;
        }
        
        
        

}
