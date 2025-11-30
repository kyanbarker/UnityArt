- in clone pattern, want to have a function `ApplyEffect(CloneIndex in [0, NumClones - 1])` that applies some operation on the clone according to the index. the result of applying to all indices in [0, NumClones - 1] is the pattern
- one example is delta transform and another example is gradients
- we want to refactor clone pattern so that we can provide arbitrary functions from int to void 
- code example
```c#
interface IEffect
{
    // applies this effect to the clone according to the index
    void Apply(int index, GameObject clone);
}

var clonePatternGameObject = new GameObject(
    "ClonePattern", typeof(ClonePattern), typeof(DeltaTransformEffect), typeof(GradientEffect)
);
var clonePattern = clonePatternGameObject.GetComponent<ClonePattern>();
// configure clone pattern

```
but wait, we need to be able to modulate gradientLength. So 
- 
- we want to change hardcoded values to suppliers
- 
- 
- 
- 
- 
- 
- 
- 
- 
- 
- 