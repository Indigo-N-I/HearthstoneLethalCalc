using System.Collections.Generic;
using System.Linq;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;
using Hearthstone_Deck_Tracker.Utility.BoardDamage;
namespace PluginExample
{
 internal class Curvy
 {
  private int _mana = 0;
  private CurvyList _list = null;
  public Curvy(CurvyList list)
  {
   _list = list;
   // Hide in menu, if necessary
   if (Config.Instance.HideInMenu && CoreAPI.Game.IsInMenu)
    _list.Hide();
  }
  internal List<Entity> Entities =>
   Helper.DeepClone<Dictionary<int, Entity>>(CoreAPI.Game.Entities).Values.ToList<Entity>();
  internal Entity Player => Entities?.FirstOrDefault(x => x.IsPlayer);
  // Reset on when a new game starts
  internal void GameStart()
  {
   _mana = 0;
        	_list.Update(new List<Card>());
  }
  // Need to handle hiding the element when in the game menu
  internal void InMenu()
  {
   if (Config.Instance.HideInMenu)
   {
    _list.Hide();
   }
  }
  // Update the card list on player's turn
  internal void TurnStart(ActivePlayer player)
  {
   if (player == ActivePlayer.Player && Player != null)
   {
    _list.Show();
    var board = CoreAPI.Game.Player.Board;
            	List<Card> cards = new List<Card>();            	
            	for( int i = 0; i < board.Count(); i++)
    	cards.Add(board.ElementAt(i).Card);
            	var boardState = new BoardState();
            	
            	if (HasLethalOnBoard())
                	_list.Update(cards);
   }
  }

  //can prob make this recursive and shorter
    	private static bool HasLethalOnBoard()
    	{
        	var boardState = new BoardState();
        	var oppHP = boardState.Opponent.Hero.Health;
        	var playerBoard = Core.Game.Player.Board;
        	var attacks = playerBoard.Where(c => c.IsInPlay && (c.IsMinion || c.IsWeapon)).Select(c => c.Attack).ToList();
        	var oppBoard = Core.Game.Opponent.Board;
        	var oppTauntHP = oppBoard.Where(c => c.HasTag(GameTag.TAUNT)).ToList()
            	.Select(c => c.Health).ToList<int>();
        	attacks.Sort();
        	oppTauntHP.Sort();
        	if (attacks.Sum() <= oppTauntHP.Sum() + oppHP || attacks.Count() <= oppTauntHP.Count())
            	return false;
        	foreach (int i in attacks)
        	{
            	if (oppTauntHP.Contains(i))
            	{
                	attacks.Remove(i);
                	oppTauntHP.Remove(i);
            	}
        	}
        	outer:  foreach (int i in oppTauntHP)
        	{
            	bool removed = false;
            	for (int maxDiff = 0; maxDiff < attacks.Max(); maxDiff++)
            	{
                	var needToRemove = findNumbers(attacks, i, maxDiff);
                	foreach (int removal in needToRemove)
                	{
                    	attacks.Remove(removal);
                    	if (!(needToRemove.Count == 0))
                    	{
                        	removed = true;
                    	}
                	}
                	if(removed)
                	{
                    	oppTauntHP.Remove(i);
                    	goto outer;
                	}
            	}
        	}
        	if(attacks.Sum() >= oppHP)
        	{
            	return true;
        	}
        	return false;
    	}
    	
    	private static List<int> findNumbers(List<int> originalList, List<int> included, int wanted, int over){ /*
    	Returns a List<int> from included that is as little over the wanted amount as possible.
    	If the numbers from included cannot add up to or over the wanted number, the included list is returned
    	*/
    	 List<int> toReturn = included;
    	 bool found = false;
    	 
    	 foreach(int i in toReturn){
       originalList.remove(i); //remove numbers already counted for
   	  }
    	 int sum = toReturn.sum();
    	 if(sum > wanted + over){
    	  return included; // no need to calculate for numbers that are already over the wanted limit
    	 }
    	 List<int> addingBase = toReturn;
    	 List<int> recursiveReturn = new List<int>();
    	 int increaseOver = 0;
    	 while(!found){ 
    	  if(originalList.size = 0){ //if there are no more numbers to add, can't add numbers
    	   break;
    	  }
    	  foreach(int i in originalList){
    	   if(sum + i <= wanted + over){ //if the sum of the numbers in the incuded list and the number that is minium over, add the number back
    	    toReturn.add(i);
    	    found = true;
    	    return toReturn;
    	   }
    	  }
    	  
    	  if(!found){
    	   foreach(int i in originalList){
    	    addingBase = toReturn;
    	    addingBase.add(i);
    	    recrusiveReturn = (originalList, addingBase, wanted, over + increaseOver); // call this again w/ one more number in the included list
    	    if(!recursiveReturn.equals(addingBase)){ //only way for this to happen is if the ideal addition has been found
    	     found = true;
    	     return recursiveReturn;
    	    }
    	   }
    	  }
    	  increaseOver++; //if the ideal combination was not found, add 1 to the acceptable margin
    	  if(increaseOver > originalList.Max(){
    	   break; //if margin of error is greater than greatest possible number added, it cannot happen
    	  }
    	 }
    	return included; //if the list cannot add up to or over desired amount, return the initial list
    }
 }
}

