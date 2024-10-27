-> mushroom_introduction
==mushroom_introduction
{& | It's you again?} What do you want? #mushroom
    + "Am I intruding?"
    + [(Leave)] -> stormed_off 
- Yes. Leave me alone. #mushroom
    + I have a question.
    + [(Leave)] -> stormed_off
- Don't you have someone else to bother? #mushroom
    + Just answer me and I'll leave you alone.
    + Please?
- Fine, what is it? #mushroom
    + Can you help me get out of here?
    + You know what? I don't care. [(Leave)] -> stormed_off
- That I can't help you with.  #mushroom
    + Oh, come on!
         Look. <>#mushroom
    + Not even a little bit?
-  All I know is that there's this old poem. #mushroom //The first of the above options gives this line a DOUBLE mushroom tag. The best I know this will not be an issue. Let's hope.
It's ancient.  #mushroom
Nobody remembers the thing.  #mushroom
    + Not even a little bit?
    + Nobody at all?
- Well, I remember a few lines of it. #mushroom
Who flouts the endless ebbs of time: #mushroom #poem
The right path they must seek. #mushroom #poem
That's all.
    + What does that mean?
        You said it was one question. #mushroom
        Leave me alone already. #mushroom 
    + Thank you.
        Will you leave me alone now? #mushroom
-   -> leaving

==stormed_off
-> leaving

==leaving
-> END