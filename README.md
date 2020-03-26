# UnityAssertionAttributes

Assertions using attributes for Unity projects

#### Example

```C#
public class MyComponent : MonoBehaviour
{
    [AssertNotNull]
    public Collider c;
}

```

Unity will not enter play mode if c is not assigned.
