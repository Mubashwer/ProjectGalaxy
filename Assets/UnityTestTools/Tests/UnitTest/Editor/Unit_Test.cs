using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnitTest
{
		using NUnit.Framework;
		
		[TestFixture]
		public class PlayerControllerTest
		{
				
		// Tests that damage is applied to the player when it is hit.
		[Test]
		public void Player_RpcDamaged_Test()
			{
			 PlayerController player;

			// Instantiating the player.
			player =  GameObject.Find("Player1").GetComponent<PlayerController>();
			// Getting current health
			float playerHealth = player.getHealth();
			float appliedDamage = 200f;
			// Damage done applied for testing RpcDamaged function
			player.RpcDamaged (appliedDamage);
			// Getting new health
			float newPlayerHealth = player.getHealth();
			// Calculating what should be the new health
			float actualDamage = playerHealth - newPlayerHealth;
			
			// assert (verify) that health was updated.
			Assert.AreEqual(appliedDamage, actualDamage);
				
			}
			
		
		// Tests that when damage is applied to a player with shield, no damage is done
		[Test]
		public void Player_ShieldPowerUp_Test()
		{
			PlayerController player2;
			
			// Instantiating the player.
			player2 =  GameObject.Find("Player2").GetComponent<PlayerController>();
			// Getting current health
			float playerHealth = player2.getHealth();
			// Extracts power up item
			player2.RpcPowerUpExtract(player2.item.powerUpName, 1);
			// AppliedDamage with powerUp
			float appliedDamage = player2.powerUp.Defend(200f);
			// Damage done applied
			player2.RpcDamaged (appliedDamage);
			// Getting new health
			float newPlayerHealth = player2.getHealth();
			// There should not be any damage due to powerUp shield
			float actualDamage = playerHealth - newPlayerHealth;
			// assert (verify) that health was updated.
			Assert.AreEqual(0f, actualDamage);
			
		}	
		
	}
	
	
	[TestFixture]
	public class EnemyAITest
		{	
		
		// Tests that damage is applied to the enemy when it is hit.
		[Test]
		public void Enemy_RpcDamaged_Test()
			{
			EnemyAI enemy;
			
			// Instantiating the enemy.
			enemy =  GameObject.FindObjectOfType<EnemyAI>();
			// Getting current health
			float enemyHealth = enemy.Health;
			float appliedDamage = 200f;
			// Damage done applied for testing RpcDamaged function
			enemy.RpcDamaged (appliedDamage);
			// Getting new health
			float newEnemyHealth = enemy.Health;
			// Calculating what should be the new health
			float actualDamage = enemyHealth - newEnemyHealth;
			
			// assert (verify) that health was updated.
			Assert.AreEqual(appliedDamage, actualDamage);
			
			}
		}
				
			    
}
