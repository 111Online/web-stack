<h1>CSS Specificity Graph</h1>
<p>This graph is a conceptual visualisation of the outputted CSS, it does not have to be perfect but gives an idea of where the Sass may be producing over-specified CSS.</p>
<p>Spikes are "bad", and the general trend should be towards higher specificity later in the stylesheet. <a href='http://csswizardry.com/2014/10/the-specificity-graph/'/>More info</a></p>

{% render '@cssgraph' %}
<script src="/components/raw/cssgraph/specificity.js"></script>
<script src="/components/raw/cssgraph/specificity-graph-standalone.js"></script>
<script src="/components/raw/cssgraph/example.js"></script>
