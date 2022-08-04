# VRChat Auto Toggle Creator
A Unity Editor tool to automatically combine multiple Animator controllers and VRC Expresison Parameters/Menus. Practical uses are for when consolidating animations from different version of the same or similar models. The tool is currently smart enought to warn you when there are duplicate properties that may need your attention and does not to any irreversable changes. <p align="right">[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/N4N06S00V)</p>

# Download
https://github.com/CascadianWorks/VRC-Animation-Combiner/releases

# How to Use
1. Download ***AnimationCombiner.cs*** from the download page linked above.
2. Drag the script anywhere into your asset folder in unity and open the menu from the top bar under Cascadian/AutoToggleCreator
4. Make sure your avatar has the VRC_Descriptor and has the FXAniamtionController, VRCExpressionsMenu and VRCExpressionParameters assets attached then click the auto fill button at the top. (Or drag in the four fields manually).
5. Next, add the controllers, parameters and menus you'll want to be mergin to the lists below.
6. Before hitting the button, make sure to chekc below the button and make sure there are no warnings. If there are warnings, make sure you change what is needed to resolve the conflict (some warnings bay be safe to ignore, use your own judgment or ask someone else for help).
7. When that is done, you can click the "Combine!" button and it will merge the animations and settings to the main ones you selected above.

# Current known Issues
- If you have too many Menu controls (more than 8?) you will not be able ot see them in game. I'll add some kind of option to account for this soon.
