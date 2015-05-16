using UnityEngine;
using System.Collections;

public class EdgeToNeighbour {
    private float cost;
    private Node neighbour;

    public EdgeToNeighbour(Node neighbour) : this(neighbour, 0.0f) { }

    public EdgeToNeighbour(Node neighbour, float cost) {
        this.cost = cost;
        this.neighbour = neighbour;
    }

    public virtual float Cost {
        get {
            return cost;
        }
    }

    public virtual Node Neighbour {
        get {
            return neighbour;
        }
    }
}
