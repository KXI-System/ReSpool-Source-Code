using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace KXI 
{
    public class ScarfVariableStorage : MonoBehaviour
    {
        private ScarfManager manager;

        private void Awake()
        {
            GetComponent<ScarfManager>();
        }
    }
}
