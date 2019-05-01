# Unity-Object-Pooler
A simple implementation of Game Object pooling mechanism for Unity

# Installation

Simply download the PoolManager.cs file into your Unity project and you are good to go.

# Usage

To instantiate a GameObject with poolmanager simply use PoolManager.Instantiate function instead of the normal Object.Instantiate function.

To destroy a GameObject with poolmanager simply use PoolManager.Destroy function. This simply sets the gameobject to be inactive and will be returned to the pool so it may be reused later.

Make sure all initialization code of the objects to be pooled are written in the OnEnable method and all code for deinitialization are written in OnDisable and NOT in Start and Destroy respectively.




# Remarks

By default the globalPoolsize is set to 20 and can be changed for each GameObjects independently by using the PoolManager.SetPoolSize function or by changing the value of the static int globalPoolSize directly. Similarly a globalNetPoolSize exists which can be used to set the total number of objects in pool including the inactive ones.

To instantiate a GameObject by growing the pool simply use PoolManager.InstantiateGrowing function.If this is not used then the first gameObject which was created in the pool which is currently active will be forcefully reused here.

Information about the GameObject Pool can be accessed by using PoolManager.GetPool function.
