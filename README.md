# Unity-Object-Pooler
A simple implementation of Game Object pooling mechanism for Unity

# Installation

Simply download the PoolManager.cs file into your Unity project and you are good to go.

# Usage

To instantiate a GameObject with poolmanager simply use PoolManager.Instantiate function which returns the instantiated gameobject.The transform and all can then be modified within the code itself.

To destroy a GameObject with poolmanager simply use PoolManager.Destroy function. This simply sets the gameobject to be inactive and will be returned to the pool so it may be reused later.

# Remarks

By default the poolsize is set to 10 and can be changed for each GameObjects independently by using the PoolManager.SetPoolSize function.

To instantiate a GameObject by growing the pool simply use PoolManager.InstantiateGrowing function.

Information about the GameObject Pool can be accessed by using PoolManager.GetPool function.
