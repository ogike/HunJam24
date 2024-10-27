//->reloop //this is some test infrastructure and should be deleted until the next comment.
//==reloop
//* [first] -> first_loop_ending 
//* [second] ->second_loop_ending
//* [third] -> third_loop_ending
//+ [fourth] ->fourth_loop_ending
//this is the next comment
{-> first_loop_ending | ->second_loop_ending | -> third_loop_ending | ->fourth_loop_ending} 

==first_loop_ending
This is the end. # pool
For now.# pool
The creators of this game are busy.# pool
They are concentrating their mental energies to open a time loop.# pool
For themselves.# pool
You know, so they have enough time to finish the whole thing they planned.# pool
If you're reading this, they might have already succeeded.# pool
Or maybe not.# pool
For now, you can still go back and find everything that you missed. # pool
Talk to a few more characters.# pool
Anyway, we're grateful you checked out our game.# pool
-> END


==second_loop_ending
Oh, you're back.#pool
We hope you found some new things.#pool
Talked to a few more folks.#pool
In the meantime, the creators may have already opened their own loop.#pool
Let's hope they're not trapped there.#pool
->END

==third_loop_ending
Hello, welcome back.#pool
We truly appreciate you taking this time with our frantic little creation.#pool
Although, we're afraid afraid we're running out of new things that you can discover.#pool
But if you're having fun, who are we to stop you?#pool
->END

==fourth_loop_ending
Hello{.|once again.}#pool
You must be having some fun with this.#pool
Or you might be {getting | quite} frustrated {|by now}.#pool
I'm here to tell you that {this is the last ending | there's no more endings} that we can offer you.#pool
//+[reloop] ->reloop //this is some test infrastructure and should be deleted until the next comment.
//+[finish] //this the next comment.
->END