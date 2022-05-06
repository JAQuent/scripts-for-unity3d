A way to track what appears where on the screen for Unity & UXF
================
Joern Alexander Quent
06/05/2022

``` r
library(knitr)
library(ggplot2)
library(plyr)
```

# Main idea behind this tool

For the type of memory experiments using virtual reality (VR) that I ran
during my PhD I would have loved to use eye tracking to ascertain that
the object that my participants encountered during these tasks were
actually seen. Unfortunately, I did not have access to a head mounted
display that also allows eye tracking at the same time. So I thought
about the second best thing because I knew there must be something. One
important functionality that games engines like unity usually offer is
to cast rays from certain point into certain direction to detect whether
this rays hits anything. This is typically invisible to the player but
is crucial for a lot of different functions (e.g. Did the player hit the
enemy when they shot at them?). This functionality also offers a way to
record what object appear where on a screen at which time. Figure 1
illustrates the basic idea.

![Figure 1: Illustration of the basic idea of the screen ray
tracker.](images/illustration.png) Figure 1: Illustration of the basic
idea of the screen ray tracker.

Shortly put, on every frame *screen ray tracker* casts rays from the
camera parallel to the screen project and detects whether or not objects
on so-called [layer
masks](https://docs.unity3d.com/ScriptReference/LayerMask.html). If an
object is hit, the ray index, the ray position as well as the name of
the game object that has been hit is saved with a time-stamp. This
tracker is implement in
[UXF’s](https://github.com/immersivecognition/unity-experiment-framework)
tracking system (see
[here](https://github.com/immersivecognition/unity-experiment-framework/wiki/Tracker-system)).
The user of this tool as full control where on the screen the rays will
project to by providing x,y coordinates form 0 to 1 in .json file for
the UXF experiment. To cut to the chase, what is this useful for?

![Figure 2: Screenshots of the *screen ray tracker* with enabled debug
mode in Unity’s scene view. The rays in red do not hit any object and
they are also not logged. Rays that hit an object in that scene are
green. In this example the floor & the walls are not part of the layer
mask, which is why they are ignored.](images/screenshots.png)

Figure 2: Screenshots of the *screen ray tracker* with enabled debug
mode in Unity’s scene view. The rays in red do not hit any object and
they are also not logged. Rays that hit an object in that scene are
green. In this example the floor & the walls are not part of the layer
mask, which is why they are ignored.

# What is possible?

There are number of possible applications of this tool. I list only a
few of them. 1. Find the first time point an object appears on the
screen. 2. Find out whether an game object ever appeared centrally on
the screen? 3. If so how long? 4. Which object stay on the screen for
the longest duration?

More specific combinations of these questions can be answered with the
data that is collected. In envision use-case, I would be able to verify
whether an object was at any given time point visible to the
participant. In naturalistic settings where the participants for
instance explores and environment on their own this not ensured.
However, it can be crucial if I want to explain why an object was
remembered well or possible forgotten. The latter would be simply
impossible to say if I don’t know if the object was ever visible to the
participant.

# Investigating example data

To give an example, I quickly analyse pilot data from a quick experiment
that I started where participant have to collect diamond-shaped object
to get reward (depicted in Figure 2).

``` r
# Load the data
screenTracker <- read.csv("exampleData.csv")

# What does the data look like?
head(screenTracker)
```

    ##       time rayIndex    x    y objectDetected
    ## 1 10.26323        2 0.25 0.05       diamond0
    ## 2 10.26323        3 0.35 0.05       diamond0
    ## 3 10.26323        4 0.45 0.05       diamond0
    ## 4 10.26323        5 0.55 0.05       diamond0
    ## 5 10.26323        6 0.65 0.05       diamond0
    ## 6 10.26323        7 0.75 0.05       diamond0

Quick explanation of the columns:

-   ***time***: The time point of the log (Unity’s Time.time).
-   ***rayIndex***: The ray index starting form 0 to how many rays were
    provided in the .json file - 1.
-   ***x***: The horizontal position on the screen (between 0 and 1).
-   ***y***: The certival position on the screen (between 0 and 1).
-   ***objectDetected***: The name of the object as it appeared in the
    game view. It might therefore be important to give unique names to
    the objects.

So what can I do with the data? I can for instance visualise where on
the screen a certain object (e.g. a game object called Diamond 8)
appeared.

``` r
# Assume 1920 x 1080 screen resolution 
screenTracker$x_pixel <- screenTracker$x*1920
screenTracker$y_pixel <- screenTracker$y*1080
  
# Where did diamond 8 appear on the screen?  
diamond8 <- screenTracker[screenTracker$objectDetected == 'diamond8', ]

# Plot
ggplot(diamond8, aes(x = x_pixel, y = y_pixel)) +  
  geom_count() +
  labs(title = "Where on the screen was Diamond 8?", x = "x pixel", y = "y pixel")
```

![](README_files/figure-gfm/where_on_screen-1.png)<!-- -->

Since, the camera’s x and z-rotation is fixed in this experiment the
observed pattern is pretty reasonable as the game object called Diamond
8 mostly appeared in the middle strip of the screen. Only when this
object was collected, other parts of the screen like the corners were
filled with the object.

Another thing I can visualise is how many time points (not time
duration) each object was on the screen and plot that distrubution.

``` r
# For each object get each time point it was somewhere on the screen
timeOnScreen <- ddply(screenTracker, c("objectDetected", "time"), summarise, samples = length(time))

# Now aggregate across time points
timeOnScreen <- ddply(timeOnScreen, c("objectDetected"), summarise, timePoints = length(time))

# Sort objectDetected by timePoints
timeOnScreen$objectDetected <- factor(timeOnScreen$objectDetected, 
                                       labels = timeOnScreen$objectDetected, 
                                       levels = timeOnScreen$objectDetected[order(timeOnScreen$timePoints)]) 

# Plot
ggplot(timeOnScreen, aes(x = timePoints, y = objectDetected)) + 
  geom_bar(stat = "identity", width = 0.5) +
  theme(axis.text.y = element_text(size = 6)) +
  labs(title = "Objects sorted by number time points on the screen", x = "Time points", y = "Object")
```

![](README_files/figure-gfm/screen_tracker_earliest_time_point-1.png)<!-- -->

Based on this distribution we learn that Icosahedron 9 is the object was
on the screen the highest number of time points. A more specific
question for naturalistic experiments could be more targeted and ask
when is the first time this object appeared on the screen?

``` r
# Find the first time icosahedron9 was on the screen.
firstTime <- round(min(screenTracker$time[screenTracker$objectDetected == 'icosahedron9']))
```

In this case, the answer is that Icosahedron 9 first appeared on the
screen on second 30. These are obviously only examples and many more
things can be assessed with this data.

# How to configure the script so it can be used?

Now, to the more technical bit on how to use this script with Unity (I
currently use Unity 2021.3.1f1c1) and UXF (version 2.4.3). Instructions
from the script itself:

> Attach this component to any game object (e.g. an empty one) and
> assign it to the correct field. It is called Tracked Objects. You can
> find this on the *Session (Script)* attached to the *\[UXF_Rig\]* game
> object under *Data collection* tab. Just drag & drop your ray casting
> game object on to the field. Furthermore, you have to add three
> methods of the *Screen Ray Tracker (Script)* to events that can be
> found undder the *Events* tab on the same *Session (Script)*. In
> particular, you need to add *GetRayCoordinates* to *On Session
> Beginn*. For *On Trail Begin*, you have add *StartRecording* and
> *StopRecording* has logically be added to *On Trial End*. This
> controls the recording, which is important as we will have choose the
> manual recording mode for this tracker.

> In the inspector under *Necessary Input* you have to add the camera
> from which you want to cast the rays and the *\[UXF_Rig\]*. Also, you
> need to make sure that the *Update Type* below is set to *Manual* (see
> Figure 3).

> The last thing that has to be provided are the ray coordinates, which
> are added to the .json file that is read in at the beginning of the
> sessiono of your experiment. This .json file can for instance look
> like this

``` json
{
"ray_x": [0.5],
"ray_y": [0.5]
}
```

> if you only want one ray in the middle. The only important thing is
> that you have to provide input in form of *ray_x* and *ray_y*. Then
> you’re good to go.

> *Optional Input* include the ability to activate *Debug mode*, which
> will print each hit to the console and visualise your rays, *Distance*
> (i.e. how far the ray is cast), which by the default is set to
> infinity but can be changed to a different value and importantly you
> can also provide *Layer Mask Names* if you want to organise your
> objects on those and only some of them should be detectabe by the
> rays.

![Figure 3: Inspector view of the script.](images/inspector.PNG)

Figure 3: Inspector view of the script.

# A word on peformance

I have not done a lot of performance testing yet but I have not seen
much of the performance decrease even if I cast 100 rays at once but
further testing might be worthwhile. Depending on how long a trial is,
it is possible that saving the tracking data at the end can take some
time.
