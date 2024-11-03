INCLUDE ./globals.ink
// intro monologue, spoken _to_ the mushroom

//->reloop //this is some test infrastructure and should be deleted until the next comment.
//==reloop
//+ [begin] -> intro //this is the next comment

-> intro
== intro
    * Where am I?
        I don't know how you got here, but you're in our forest.
    * Who am I?
        If you don't know that, I can't help you! #mushroom
    * What am I doing here?
        I should ask this question. #mushroom
        It's you who came into our woods. #mushroom
    * {visit_count > 0} Why am I here again?
        Again? I've never seen this pale face of yours. #mushroom
    * {visit_count > 1} Is there no escape from this?
        Escape? #mushroom
        Good. #mushroom
        I don't like your face. #mushroom
    * {visit_count > 2} Am I stuck here forever?
        Forever? #mushroom
        You just got here. #mushroom
        And I don't like your pale face. #mushroom
    * ->leave
- You {&should|better} leave. #mushroom
You don't belong {&in these woods|here}. #mushroom
->leave
=leave

//+   [restart] ->reloop
//+   [finish]
-> END