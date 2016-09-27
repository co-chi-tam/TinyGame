require('./utils/UtilJS')(); 

exports.setRequestUrl = function(app, db, wss){
    var user = require('./controllers/controller_user')
        ,sodier = require('./controllers/controller_sodier')
        ,mercenary = require('./controllers/controller_mercenary')
        ,map = require('./controllers/controller_map')
		,item = require('./controllers/controller_item')
		,shop = require('./controllers/controller_shop')
		,inventory = require('./controllers/controller_inventory')
		,battleLog = require('./controllers/controller_battleLog')
		,luckyWheel = require('./controllers/controller_luckyWheel')
		,token = require('./controllers/controller_token')
		,chat = require('./controllers/controller_chat');
		
	user.init(db);
	sodier.init(db);
	mercenary.init(db, wss);
	map.init(db);
	item.init(db);
	shop.init(db, wss);
	inventory.init(db);
	battleLog.init(db, wss);
	luckyWheel.init(db, wss);
	token.init (db);
	chat.init(db, wss);

	// User
    app.get('/api/connect_pvp',		user.getAllPVPConnects);
    app.post('/login', 				user.postLogin);
    app.post('/logout', 			user.postLogout);
	app.get('/view/register',		user.viewRegister);
    app.post('/register', 			user.postRegister);
	app.post('/change_password', 	user.postChangePassword);
	app.post('/active_user', 		user.postActiveUser);
	app.post('/complete_tutorial_user',  user.postCompleteTutorialUser);
	
	// Sodier
    // app.get('/api/all_sodier', 		sodier.getAllSodiers);
    // app.get('/api/sodier', 			sodier.getSodier);
	
	// Mercenary
    app.get('/api/all_mercenary',		mercenary.getAllMercenaries);
    // app.get('/api/mercenary', 			mercenary.getMercenary);
	// app.post('/api/create/mercenary', 	mercenary.postCreateMercenary);
	app.post('/api/update/dismiss/mercenary', mercenary.postDismissMercenary);
	app.post('/api/update/level_up/mercenary', mercenary.postLevelUpMercenary);
	app.get('/api/level_up/rate', 		mercenary.getLevelRate);

	// Map
    // app.get('/api/map', 			map.getMap);
    app.get('/api/map_formation',	map.getMapFormation);
    app.get('/api/user_map_formation',	map.getUserMapFormation);
    app.get('/api/pvp_map_formation',	map.getPVPMapFormation);
    // app.post('/api/create/user_map_formation',	map.postCreateUserMapFormation);
    app.post('/api/update/user_map_formation',	map.postUpdateUserMapFormation);
	
	// Item
	app.get('/api/all_item', item.getAllItem);
	app.get('/api/item', item.getItem);
	
	// Shop
	app.get('/api/shop', shop.getAllItemInShop);
	app.post('/api/buy_item_gold', shop.postBuyItemByGoldInShop);
	
	// Inventory
	app.get('/api/inventory', inventory.getAllItemInInventory);
	app.post('/api/use/inventory', inventory.postUseItemInInventory);

	// Battle Log
	app.get('/api/battle_log', 			battleLog.getAllBattleLog);
	app.post('/api/create/battle_log', 	battleLog.postBattleLog);
	app.post('/api/update/battle_log', 	battleLog.postClaimAwardBattleLog);
	
	// Lucky Wheel
	app.post('/api/scroll/lucky_wheel', 	luckyWheel.postScrollLuckWheel);
	app.get('/api/time/lucky_wheel', 		luckyWheel.getTimeScrollLuckWheel);
	
	// Token
	app.get('/api/good_login_token', 	token.getGoodLoginToken);
	
	// Chat
	app.get('/chat', chat.getChat);

}

















