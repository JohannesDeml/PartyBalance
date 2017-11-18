using System;
using UnityEngine;

namespace Supyrb
{
    [Serializable]
    public class AnimParameter
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private int hash = -1;

	    public string Name
	    {
		    get { return name; }
	    }

	    public int Hash
        {
            get { return hash; }
        }

        public AnimParameter(string name)
        {
            this.name = name;
			hash = Animator.StringToHash(name);
		}
    }
}