using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace VET.Movement
{
    /* 
     * Author: Mark McGill (Glasgow)
     *
     * Enables translational gain based on a tracked object. Does so by moving the SteamVR_PlayArea.
     * In this way, the safety boundaries relative to the real physical area are preserved.
     *
     * N.b. There is also now a VRTK add on for applying translational gain which might be more general purpose use.
     * See: https://vrtoolkit.readme.io/docs/vrtk_roomextender
     */
    public class TranslationalGain : MonoBehaviour
    {
        [Header("Object whose position should be tracked for the accelerated movement (e.g. HMD, controller, tracker...)")]
        public GameObject trackedObjectInPlayArea;

        [Header("Gain to be applied. 0 = no gain; 1 = 100% gain.")]
        public float gainRatio = 0.2f;

        [Header("Objects that should be scaled in position/size to match gain (e.g. obstacles)")]
        public List<SceneObject> managedSceneObjects = new List<SceneObject>();

        public SteamVR_PlayArea playArea;
        private Vector3 playAreaCenter;

        #region Unity lifecycle methods
        public void Start()
        {
            if (playArea == null)
                playArea = GameObject.FindObjectOfType<SteamVR_PlayArea>();

            if (playArea == null)
                Debug.LogError("SteamVR_PlayArea not set and couldn't be found in the current scene.");

            playAreaCenter = playArea.transform.position;
            setGainRatio(gainRatio);
        }

        public void Update()
        {
            if (trackedObjectInPlayArea != null && playArea != null)
                movePlayArea();
        }
        #endregion

        #region Set / Update translational gain
        private void movePlayArea(bool ignoreY = true)
        {
            Vector3 newPosition = getPositionBasedOnRatio(playAreaCenter, trackedObjectInPlayArea.transform.localPosition, true, false);

            //Debug.Log("movePlayArea called: new position=" + newPosition + " Current position=" + playArea.transform.position);

            Vector3 newPositionExclY = new Vector3(newPosition.x, playAreaCenter.y, newPosition.z);
            playArea.transform.position = newPositionExclY;
        }

        public void setGainRatio(float newRatio)
        {
            gainRatio = newRatio;

            foreach (SceneObject so in managedSceneObjects)
            {
                so.setRatio(gainRatio);
            }
        }

        private Vector3 getPositionBasedOnRatio(Vector3 originalPosition, Vector3 offsetToPlayArea, bool ignoreY = true, bool invert = false)
        {
            Vector3 offset = offsetToPlayArea * gainRatio * (invert ? -1 : 1);

            Vector3 newPosition = originalPosition + offset;

            newPosition = new Vector3(newPosition.x, ignoreY ? originalPosition.y : newPosition.y, newPosition.z);

            return newPosition;
        }

        private Vector3 getWorldPositionBasedOnRatio(Vector3 originalPosition, bool ignoreY = true)
        {
            return getPositionBasedOnRatio(originalPosition, (originalPosition - playAreaCenter));
        }
        #endregion

        #region Manage scene objects that should be scaled based on gain (e.g. obstacles)
        private void addObjectToManage(GameObject go)
        {
            SceneObject so = new SceneObject(go);
            managedSceneObjects.Add(so);
            so.setRatio(gainRatio);
        }

        private void removeObjectToManage(GameObject go)
        {
            SceneObject item = managedSceneObjects.FirstOrDefault(x => x.managedObject == go);
            if (item != null)
            {
                managedSceneObjects.Remove(item);
                item.setRatio(1.0f);
            }
        }
        #endregion

        /// <summary>
        /// A scene object is any gameobject that should either be moved or rescaled based on the current level of translational gain.
        /// 
        /// For example, in our study we tested movement toward targets in different sized virtual spaces, but we wanted obstacles to
        /// remain in the same position proportionally, and still cover the same area proportionally.
        /// </summary>
        [Serializable]
        public class SceneObject
        {
            public bool shouldRearrange = true;
            public bool shouldRescale = true;
            public GameObject managedObject;

            private Vector3 originalPosition;
            private Vector3 originalScale;
            private bool isSetup = false;

            TranslationalGain pimove;

            public SceneObject(GameObject go, bool rearrange = true, bool rescale = true)
            {
                managedObject = go;
                shouldRearrange = true;
                shouldRescale = true;
                setup();
            }

            /// <summary>
            /// If the scene object has been created in the Unity Inspector...
            /// </summary>
            private void setup()
            {
                originalPosition = managedObject.transform.position;
                originalScale = managedObject.transform.localScale;
                pimove = GameObject.FindObjectOfType<TranslationalGain>();
                isSetup = true;
            }

            public void setRatio(float ratio)
            {
                float multiplier = 1 + ratio;

                if (!isSetup)
                    setup();

                if (shouldRescale)
                    managedObject.transform.localScale = new Vector3(originalScale.x * multiplier, originalScale.y, originalScale.z * multiplier);


                if (shouldRearrange)
                    managedObject.transform.position = pimove.getWorldPositionBasedOnRatio(originalPosition);
            }
        }
    }
}
