---
layout: post
title:  "Cellular Automation"
date:   2019-07-23
categories: jekyll update
---
> A cellular automaton is a collection of "colored" cells on a grid of specified shape that evolves through a number of discrete time steps according to a set of rules based on the states of neighboring cells. The rules are then applied iteratively for as many time steps as desired.
>~ [from Wolfram MathWorld][cellular-automation-wolfram-mathworld]
<br>

The goal of this [proof-of-concept][cellular-automation] was to demonstrate that a convincing enough liquid simulation can be created without using a physics library but simply by running a cellular automation to move the "liquid" in a grid based game. 
<br>

{% include cellular-automation.html %}
<br>

<br>

This simulation runs on 200x200 grid of cells represented by the 2-dimensional array `byte[,] cells`. An empty cell is `0`, filled cell is `1`, and blocked cell is `-1`. 
The simplest logic is basically to move the "liquid" to an empty neihboring cell. 
<br>
{% highlight csharp %}
if (cells[x, y - 1] == 0)
{
    cells[x, y - 1] = 1; // move down
}
else if (cells[x + 1, y - 1] == 0)
{
    cells[x + 1, y - 1] = 1; // move down right
}
else if (cells[x - 1, y - 1] == 0)
{
    cells[x - 1, y - 1] = 1; // move down left
}
else if (cells[x + 1, y] == 0)
{
    cells[x + 1, y] = 1; // move right
}
else if (cells[x - 1, y] == 0)
{
    cells[x - 1, y] = 1; // move left
}
{% endhighlight %}
<br>
The same logic can be re-written for a better readability by declaring these constants:
<br>
{% highlight csharp %}
public const sbyte BLOCKED = -1;
public const sbyte EMPTY = 0;
public const sbyte FILLED = 1;
{% endhighlight %}
and by introducing a helper method:
{% highlight csharp %}
private bool FillIfEmpty(int x, int y)
{
    bool isEmpty = x >= 0 && x < width && y >= 0 && y < height && cells[y, x] == EMPTY;
    if (isEmpty)
    {
        cells[y, x] = FILLED;
    }
    return isEmpty;
}
{% endhighlight %}
<br>
The block above becomes more concise and much easier to follow:
<br>
{% highlight csharp %}
if (FillIfEmpty(x, y - 1)) {}
else if (FillIfEmpty(x + 1, y - 1)) {}
else if (FillIfEmpty(x - 1, y - 1)) {}
else if (FillIfEmpty(x + 1, y)) {}
else if (FillIfEmpty(x - 1, y)) {}
else {cells[y, x] = 1;}
{% endhighlight %}
<br>

The easiest way to render the cells was to use Unity's `ParticleSystem` which allows to set the positions of the particle to the values calculaated by the cellular autimation. 
<br>

{% highlight csharp %}
private void UpdateParticles(ref sbyte[,] cells)
{
    if (particleSystem.particleCount < cells.Length)
    {
        particleSystem.Emit(emitParams, cells.Length - particleSystem.particleCount);
        particleSystem.GetParticles(particles);
    }

    Vector3 position = Vector3.zero;
    int i = 0;
    int width = cells.GetLength(0);
    int height = cells.GetLength(1);
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (cells[y, x] != FILLED)
            {
                continue;
            }

            position.x = x;
            position.y = y;
            if (i < particles.Length)
            {
                particles[i].position = position;
            }

            i++;
        }
    }

    for (; i < particles.Length; i++)
    {
        particles[i].remainingLifetime = 0f;
    }

    particleSystem.SetParticles(particles);
}
{% endhighlight %}

The project's source code can be found [here][cellular-automation].

[cellular-automation]: https://github.com/burusov/burusov.github.io/tree/main/cellular-automation
[cellular-automation-wolfram-mathworld]: https://mathworld.wolfram.com/CellularAutomaton.html#:~:text=A%20cellular%20automaton%20is%20a,many%20time%20steps%20as%20desired.
