using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beluga.AudioShit
{
    public class BelugaVoicelineManager : MonoBehaviour
    {
        public bool depthAlreadyPassed = false;
        public bool depthLessAlreadyPassed = false;

        public bool liveAlreadyPassed = false;
        public bool liveLessAlreadyPassed = false;

        public bool energyZeroPassed = false;
        public bool energyCriticalPassed = false;
        public bool energyDepletingPassed = false;
        public bool energyLowPassed = false;

        public float timeTillNewLeviathanDetect = 0f;


        public List<string> voicelineQueue = new List<string>();
        public bool isVoicelinePlaying = false;

        public void CheckForVoicelines()
        {
            // references
            var beluga = gameObject.GetComponent<Beluga>();


            // depth shit
            int depthThreshhold = 20;
            int currentDepth;
            int crushDepth;
            beluga.GetDepth(out currentDepth, out crushDepth);
            if (currentDepth > crushDepth)
            {
                if (!depthAlreadyPassed)
                {
                    depthAlreadyPassed = true;
                    AddVoicelineToQueue("DepthMaximum");
                }
            }
            else if (currentDepth > crushDepth - depthThreshhold)
            {
                if (!depthLessAlreadyPassed)
                {
                    depthLessAlreadyPassed = true;
                    AddVoicelineToQueue("DepthClose");
                }
                depthAlreadyPassed = false;
            }
            else
            {
                depthAlreadyPassed = false;
                depthLessAlreadyPassed = false;
            }


            // health shit
            float liveCriticalThreshhold = 10f;
            float liveModerateThreshhold = 40f;
            if (beluga.GetPercentageOfHealth() < liveCriticalThreshhold)
            {
                if (!liveAlreadyPassed)
                {
                    liveAlreadyPassed = true;
                    AddVoicelineToQueue("DamageCritical");
                }
            }
            else if (beluga.GetPercentageOfHealth() < liveModerateThreshhold)
            {
                if (!liveLessAlreadyPassed)
                {
                    liveLessAlreadyPassed = true;
                    AddVoicelineToQueue("DamageModerate");
                }
                liveAlreadyPassed = false;
            }
            else
            {
                liveAlreadyPassed = false;
                liveLessAlreadyPassed = false;
            }


            // power shit
            float energyLowThreshhold = 50f;
            float energyDepletingThreshhold = 25f;
            float energyCriticalThreshhold = 10f;

            if (beluga.GetPercentageOfPower() < 1)
            {
                if (!energyZeroPassed)
                {
                    energyZeroPassed = true;
                    AddVoicelineToQueue("PowerZero");
                    
                }
            }
            else if (beluga.GetPercentageOfPower() < energyCriticalThreshhold)
            {
                if (!energyCriticalPassed)
                {
                    energyCriticalPassed = true;
                    AddVoicelineToQueue("PowerCritical");
                }
                energyZeroPassed = false;
            }
            else if (beluga.GetPercentageOfPower() < energyDepletingThreshhold)
            {
                if (!energyDepletingPassed)
                {
                    energyDepletingPassed = true;
                    AddVoicelineToQueue("PowerDepleting");
                }
                energyZeroPassed = false;
                energyCriticalPassed = false;
            }
            else if (beluga.GetPercentageOfPower() < energyLowThreshhold)
            {
                if (!energyLowPassed)
                {
                    energyLowPassed = true;
                    AddVoicelineToQueue("PowerLow");
                }
                energyZeroPassed = false;
                energyCriticalPassed = false;
                energyDepletingPassed = false;
            }
            else
            {
                energyZeroPassed = false;
                energyCriticalPassed = false;
                energyDepletingPassed = false;
                energyLowPassed = false;
            }
        }

        public void DetectLeviathan()
        {
            if (timeTillNewLeviathanDetect <= 0f)
            {
                // in seconds
                timeTillNewLeviathanDetect = 60f;

                int random = UnityEngine.Random.Range(1, 1000);

                if (random > 500)
                {
                    AddVoicelineToQueue("DetectLeviathan");
                }
                else
                {
                    AddVoicelineToQueue("DetectLifeform");
                }
            }
        }

        public void FixedUpdate()
        {
            if (timeTillNewLeviathanDetect < 0f)
            {
                timeTillNewLeviathanDetect = 0f;
            }
            else
            {
                timeTillNewLeviathanDetect -= Time.deltaTime;
            }
        }

        public void AddVoicelineToQueue(string voiceline)
        {
            voicelineQueue.Add(voiceline);
        }

        public IEnumerator PlaySubVoiceline(string voiceline, float length)
        {
            var beluga = gameObject.GetComponent<Beluga>();

            if (!beluga.isScuttled && (Player.main.GetVehicle() as Beluga) != null)
            {
                Transform position = beluga.voicelinesLoc;

                BelugaUtils.PlayFMODSound(voiceline, position);

                isVoicelinePlaying = true;

                yield return new WaitForSeconds(length);

                isVoicelinePlaying = false;
            }

            yield return null;
        }

        public void Update()
        {
            if (isVoicelinePlaying) return;

            if (voicelineQueue.Count == 0) return;

            if (voicelineQueue[0] == null) return;

            string sound = voicelineQueue[0];

            bool flag = false;
            if (sound == "FirstCraft" || sound == "FirstAgility") flag = true;

            if (flag)
            {
                BelugaUtils.PlayFMODSound(sound, Player.main.transform);
            }
            else
            {
                UWE.CoroutineHost.StartCoroutine(PlaySubVoiceline(sound, GetVoicelineLength(sound)));
            }

            voicelineQueue.RemoveAt(0);
        }

        public float GetVoicelineLength(string voiceline)
        {
            float result = 1f;
            VoicelinesInfo.infoDic.TryGetValue(voiceline, out result);

            return result;
        }

        public void PlayEntryVoiceline(float wait)
        {
            UWE.CoroutineHost.StartCoroutine(WaitAndPlayVoiceline(wait));
        }

        public IEnumerator WaitAndPlayVoiceline(float time)
        {
            yield return new WaitForSeconds(time);

            AddVoicelineToQueue("WelcomeAboard");

            yield return null;
        }
    }
}
