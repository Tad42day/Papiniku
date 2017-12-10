import 'System'
import 'UnityEngine'
import 'Assembly-CSharp'


function Start()

	--Debug.Log("Teste lua")	
	
end

function Update()

	--Debug.Log("Teste tempo lua")
	
	
	--jogador:Shoot()
	
	if jogador:SensorDeInimigo() ~= nil then
		inimigo = jogador:SensorDeInimigo()		
		posicaoInimigo = inimigo:GetPos()
		--GET OUT OF MY HEAD GET OUT OF MY HEAD GET OUT OF MY HEAD
		Debug.Log("P1 | Posicao Inimigo: " .. posicaoInimigo.x)
		
	end
	if jogador:SensorDeCol() then
		direcao = (direcao + 1)%9
	end
	
	Move(direcao)
	
	--jogador:SensorDeCol()
	--jogador:SensorDeInimigo()
	--jogador:SensorDeBala()
	--jogador:SensorDeCranios()
	--jogador:posicaoBala()
end

function VerInimigo()

	

end