from __future__ import print_function
import random
import math
import copy
#from termcolor import colored
import sys
import time
import heapq

def main():
	#############
	#############

	#GENERATOR SETTINGS:
	size_x = 6
	size_y = 6
	num_tries = 2000
	complexity = 5
	min_len = 20
	ratio = 1.8
	"""size_x = 8
	size_y = 8
	num_tries = 2000
	complexity = 8
	min_len = 20
	ratio = 1.7"""

	##############
	##############
	
	#b.generate_level()
	#b.generate_test_level()
	seed = random.randint(0,1000000)
	j = 0
	while j < 1000:
		f = open('level'+str(j+1)+'.txt', 'w')
		found_lvl = False
		b = Board(size_x,size_y)
		for i in xrange(0,num_tries):
			print(i)
			seed = random.randint(0,1000000)
			#seed = 548983
			if b.build_level(seed,complexity,min_len,ratio):
				print(seed)
				print("exportable:")
				b.export_level(f)
				found_lvl = True
				j+=1
				"""j += 1
				print("J:",j)
				if j % 10 == 0:
					ratio += 0.05
					ratio = min(1.7,ratio)
				if j % 10 == 0:
					min_len += 1
					min_len = min(35,min_len)
				if j % 50 == 0:
					complexity += 1
				if j % 64 == 0:
					size_x += 1
					size_y += 1
					size_x = min(size_x,9)
					size_y = min(size_y,9)"""
				break
			b.clean_grid()	
		#b.print_board()
		#print(b.can_reach(0,0,size_x - 1,size_y - 1,[0,0,0]),len(b.can_reach(0,0,size_x - 1,size_y - 1,[0,0,0])) - 1,b.solution_quality(b.can_reach(0,0,size_x - 1,size_y - 1,[0,0,0])))
		#print(b.blocks_passed_through(b.can_reach(0,0,size_x - 1,size_y - 1,[0,0,0])))
	#print(i)
		if found_lvl:
			b.print_board()
			f.close()
			print(b.can_reach(b.start_x,b.start_y,b.end_x,b.end_y,[0,0,0]),
				len(b.can_reach(b.start_x,b.start_y,b.end_x,b.end_y,[0,0,0])) - 1,
				b.solution_quality(b.can_reach(b.start_x,b.start_y,b.end_x,b.end_y,[0,0,0])))



class Board:
	def __init__(self,size_x, size_y):
		self.size_x = size_x
		self.size_y = size_y
		self.background_color = [0,0,0]
		self.grid = []
		self.enemies = []
		self.start_x = 0
		self.start_y = 0
		self.end_x = 0
		self.end_y = 0
		for i in xrange(size_x):
			row = []
			for j in xrange(size_y):
				row.append(["empty",[0,0,0]])
			self.grid.append(row)
	def clean_grid(self):
		self.grid = []
		for i in xrange(self.size_x):
			row = []
			for j in xrange(self.size_y):
				row.append(["empty",[0,0,0]])
			self.grid.append(row)
	def generate_level(self):
		for i in xrange(self.size_x):
			for j in xrange(self.size_y):
				r = random.random()
				color = [round(random.random()),round(random.random()),round(random.random())]
				if color == [0,0,0]:
					color[random.randint(0,2)] = 1
				if r > 0.5:
					self.grid[i][j] = ["block",list(color)]
				if r > 0.8:
					self.grid[i][j] = ["switch",list(color)]
		self.grid[0][0] = ["empty",[0,0,0]]
		self.grid[self.size_x - 1][self.size_y - 1] = ["empty",[0,0,0]]
	def generate_test_level(self):
		#orginal test
		"""self.grid[3][4] = ["block",[1,1,1]]
		self.grid[3][3] = ["block",[1,1,1]]
		self.grid[3][2] = ["block",[1,1,1]]
		self.grid[3][1] = ["block",[1,0,0]]
		self.grid[3][0] = ["block",[1,0,1]]
		self.grid[2][3] = ["block",[1,0,0]]
		self.grid[2][2] = ["switch",[1,0,0]]"""
		#first problem
		"""self.grid[0][1] = ["block",[1,0,0]]
		self.grid[1][0] = ["switch",[0,1,0]]
		self.grid[1][3] = ["block",[0,1,1]]
		self.grid[1][4] = ["block",[0,1,1]]
		self.grid[2][2] = ["block",[0,1,0]]
		self.grid[3][1] = ["block",[1,0,0]]
		self.grid[4][0] = ["block",[0,1,1]]"""
		#second problem
		"""self.grid[1][2] = ["block",[0,1,0]]
		self.grid[2][1] = ["block",[0,0,1]]
		self.grid[1][1] = ["block",[1,0,0]]
		self.grid[0][1] = ["block",[1,0,0]]
		self.grid[2][0] = ["switch",[0,1,0]]
		self.grid[1][0] = ["switch",[1,0,0]]"""
		"""self.grid[2][2] = ["block",[1,1,1]]
		self.grid[2][1] = ["block",[1,1,1]]
		self.grid[2][0] = ["block",[1,1,1]]
		self.grid[0][2] = ["block",[1,0,0]]
		self.grid[1][2] = ["block",[1,0,1]]
		self.grid[2][2] = ["block",[1,0,0]]"""
		#self.grid[5][4] = ["switch",[1,0,0]]
		#self.grid[4][5] = ["switch",[1,0,0]]
		#self.grid[5][4] = ["switch",[1,0,0]]

	def switches_pressed(self, path):
		pressed = 0
		for step in path:
			if self.grid[step[0]][step[1]][0] == "switch":
				pressed += 1
		return pressed

	def solution_quality(self,path):
		#return self.blocks_passed_through(path)
		switches_occurred = {}
		score = 0
		step_num = 0
		last_block_passed = -1
		for step in path:
			switch_mul = 1
			if self.grid[step[0]][step[1]][0] == "switch":
			#if self.grid[step[0]][step[1]][0] == "switch":
				if step in switches_occurred.keys():
					a = step_num - switches_occurred[step][0]
					b = 1
					if a <= 1:
						a = 0.2
						b = 0.3
					switch_mul  += a * switches_occurred[step][1]
					switches_occurred[step] = (step_num,switches_occurred[step][1]+b)
				else:
					switch_mul  += 4
					switches_occurred[step] = (step_num,1)
				step_num += 1
			score += 0.1 + switch_mul
		if len(path) == 0:
			return 0
		else:
			return score/float(len(path))
			
	def blocks_passed_through(self,path):
		#return 0
		last_block_color = [-1,-1,-1]
		num_unique_blocks = 0
		for step in path:
			if self.grid[step[0]][step[1]][0] == "block":
				if not self.grid[step[0]][step[1]][1] == last_block_color:
					last_block_color = self.grid[step[0]][step[1]][1]
					num_unique_blocks+=1
		return num_unique_blocks

	def path_quality(self,path):
		switches_occurred = {}
		score = 0
		step_num = 0
		for step in path:
			switch_mul = 1
			if self.grid[step[0]][step[1]][0] == "switch":
				if step in switches_occurred.keys():
					a = step_num - switches_occurred[step][0]
					b = 1
					if a <= 1:
						a = 0.2
						b = 0.3
					switch_mul  += a * switches_occurred[step][1]
					switches_occurred[step] = (step_num,switches_occurred[step][1]+b)
				else:
					switch_mul  += 1.5
					switches_occurred[step] = (step_num,1)
				step_num += 1
			score += switch_mul
		return score

	def build_level(self,seed,complexity,min_len,ratio):
		random.seed(seed)
		#we'll see
		#first we make the level more complicated then we make it solveable
		empty_blocks = []
		for i in xrange(self.size_x):
			for j in xrange(self.size_y):
				empty_blocks.append([i,j])
		"""empty_blocks.remove([0, 0])
		empty_blocks.remove([self.size_x - 1, self.size_y - 1])"""

		(start_x,start_y) = empty_blocks[random.randint(0,len(empty_blocks) - 1)]
		empty_blocks.remove([start_x,start_y])
		(end_x,end_y) = empty_blocks[random.randint(0,len(empty_blocks) - 1)]
		empty_blocks.remove([end_x,end_y])

		self.start_x = start_x
		self.start_y = start_y

		self.end_x = end_x
		self.end_y = end_y

		added_blocks = []

		previous_level = None
		current_level = None

		blocks_on_board = []
		lax_const = 0.8

		#fill

		for k in xrange(0,1000):
			t_start = time.clock()
			old_soln = self.can_reach(start_x,start_y,end_x,end_y,[0,0,0])
			if len(old_soln) >= min_len and self.solution_quality(old_soln) > ratio and k >= complexity:
				break

			#shuffle the board a little
			for i in xrange(100):
				#add some new moves to the generator
				#swap a block
				if len(blocks_on_board) > 0 and random.random() > 0:
					if len(empty_blocks) > 0:
						nPos = empty_blocks[random.randint(0,len(empty_blocks) - 1)]
						oPos = blocks_on_board[random.randint(0,len(blocks_on_board) - 1)]
						old_b = list(self.grid[oPos[0]][oPos[1]])
						self.grid[oPos[0]][oPos[1]] = ["empty",[0,0,0]]
						self.grid[nPos[0]][nPos[1]] = old_b
						soln = self.can_reach(start_x,start_y,end_x,end_y,[0,0,0])
						if self.solution_quality(soln) > self.solution_quality(old_soln) and len(soln) >= len(old_soln) * lax_const and self.blocks_passed_through(soln) >= self.blocks_passed_through(old_soln) * lax_const:
							empty_blocks.remove(nPos)
							empty_blocks.append(oPos)
							blocks_on_board.remove(oPos)
							blocks_on_board.append(nPos)
						else:
							self.grid[nPos[0]][nPos[1]] = ["empty",[0,0,0]]
							self.grid[oPos[0]][oPos[1]] = old_b
				#change the color of a block
				if len(blocks_on_board) > 0 and random.random() > 0:
					oPos = blocks_on_board[random.randint(0,len(blocks_on_board) - 1)]
					o_col = self.grid[oPos[0]][oPos[1]][1]
					if self.grid[oPos[0]][oPos[1]][0] == "switch":
						col = [0,0,0]
						col[random.randint(0,2)] = 1
					else:
						col = [round(random.random()),round(random.random()),round(random.random())]
						if col == [0,0,0]:
							col[random.randint(0,2)] = 1
						"""if col == [1,1,1]:
							col[random.randint(0,2)] = 0"""
					self.grid[oPos[0]][oPos[1]][1] = col
					soln = self.can_reach(start_x,start_y,end_x,end_y,[0,0,0])
					#if not self.switches_pressed(soln) >= self.switches_pressed(old_soln) and len(soln) >= len(old_soln):
					if self.solution_quality(soln) > self.solution_quality(old_soln) and len(soln) >= len(old_soln) * lax_const:
						pass
					else:
						self.grid[oPos[0]][oPos[1]][1] = o_col

			#self.print_board()
			#print(self.can_reach(0,0,3,3,[0,0,0]))
			added_blocks = []
			fail_time = 150
			while len(self.can_reach(start_x,start_y,end_x,end_y,[0,0,0])) > 0:
				fail_time -= 1
				if fail_time == 0:
					#print("Could not make level ;")
					#self.print_board()
					return False
				block_pos = None
				if (len(empty_blocks) == 0 and random.random() > 0.8) and len(added_blocks) > 0:
					if len(added_blocks) == 1:
						block_pos = added_blocks[0]
					else:
						block_pos = added_blocks[random.randint(0,len(added_blocks) - 1)]
				elif len(empty_blocks):
					if len(empty_blocks) == 1:
						block_pos = empty_blocks[0]
					else:
						block_pos = empty_blocks[random.randint(0,len(empty_blocks) - 1)]
					added_blocks.append(block_pos)
					empty_blocks.remove(block_pos)
				if block_pos is not None:
					if random.random() > 0.5:
						#try a block
						col = [round(random.random()),round(random.random()),round(random.random())]
						if col == [0,0,0]:
							col[random.randint(0,2)] = 1
						self.grid[block_pos[0]][block_pos[1]] = ["block", col]
					else:
						#try a switch
						col = [0,0,0]
						col[random.randint(0,2)] = 1
						#check to see if it made a short cut
						self.grid[block_pos[0]][block_pos[1]] = ["switch",col]
						soln = self.can_reach(start_x,start_y,end_x,end_y,[0,0,0])
						if len(soln) > 0:
							self.grid[block_pos[0]][block_pos[1]] = ["empty",[0,0,0]]
							added_blocks.remove(block_pos)
							empty_blocks.append(block_pos)

			"""if time.clock() - t_start > 1:
				self.print_board()"""
			#remove non essential blocks
			for bpos in added_blocks:
				old_block = list(self.grid[bpos[0]][bpos[1]])
				self.grid[bpos[0]][bpos[1]] = ["empty",[0,0,0]]
				if self.can_reach(start_x,start_y,end_x,end_y,[0,0,0]):
					self.grid[bpos[0]][bpos[1]] = old_block
					blocks_on_board.append(bpos)
				else:
					empty_blocks.append(bpos)
					#added_blocks.remove(block_pos)

			#print("yo")
			"""print(self.can_reach(0,0,3,3,[0,0,0]))"""
			fail_time = 150
			added_blocks = []
			max_switch_placements = 150
			#print("cr",len(self.can_reach(0,0,self.size_x-1,self.size_y-1,[0,0,0])))
			if random.random() > 0.5:
				tolerate = False
			else:
				tolerate = True
			while len(self.can_reach(start_x,start_y,end_x,end_y,[0,0,0])) < 1:
				fail_time -= 1
				if fail_time == 0:
					#print("Could not make level -")
					#self.print_board()
					return False
				block_pos = None
				
				if (len(empty_blocks) == 0 and random.random() > 0.7) and len(added_blocks) > 0:
					if len(added_blocks) == 1:
						block_pos = added_blocks[0]
					else:
						block_pos = added_blocks[random.randint(0,len(added_blocks) - 1)]
				elif len(empty_blocks) > 0:
					if len(empty_blocks) == 1:
						block_pos = empty_blocks[0]
					else:
						block_pos = empty_blocks[random.randint(0,len(empty_blocks) - 1)]
					added_blocks.append(block_pos)
					empty_blocks.remove(block_pos)
				if block_pos is not None:
					col = [0,0,0]
					col[random.randint(0,2)] = 1
					#check to see if it made a short cut
					self.grid[block_pos[0]][block_pos[1]] = ["switch",col]
					soln = self.can_reach(start_x,start_y,end_x,end_y,[0,0,0])
					#self.grid = backup_grid
					#if (len(soln) > len(old_soln) or (len(soln) == len(old_soln) and tolerate)) and self.switches_pressed(soln) > self.switches_pressed(old_soln):
					if self.solution_quality(soln) > self.solution_quality(old_soln) and len(soln) >= len(old_soln) * lax_const:
						self.grid[block_pos[0]][block_pos[1]] = ["switch",col]
					else:
						self.grid[block_pos[0]][block_pos[1]] = ["empty",[0,0,0]]
						#previous_level[block_pos[0]][block_pos[1]] = ["empty",[0,0,0]]
						added_blocks.remove(block_pos)
						empty_blocks.append(block_pos)

				if len(added_blocks) > max_switch_placements:
					rblock = added_blocks[random.randint(0,len(added_blocks) - 2)]
					self.grid[rblock[0]][rblock[1]] = ["empty",[0,0,0]]
					empty_blocks.add(rblock)

			#if time.clock() - t_start > 1:
				#self.print_board()
			#remove non essential blocks
			for bpos in added_blocks:
				old_block = list(self.grid[bpos[0]][bpos[1]])
				self.grid[bpos[0]][bpos[1]] = ["empty",[0,0,0]]

				soln = self.can_reach(start_x,start_y,end_x,end_y,[0,0,0])
				#used to be len()
				if self.path_quality(soln) < self.path_quality(old_soln) or len(soln) < len(old_soln) * lax_const:
					self.grid[bpos[0]][bpos[1]] = old_block
					blocks_on_board.append(bpos)
					#previous_level[bpos[0]][bpos[1]] = 
				else:
					#current_level[bpos[0]][bpos[1]] = ["empty",col]
					empty_blocks.append(bpos)
			print("iteration:",k," time:", time.clock() - t_start)


		max_enemies = random.randint(1,4)
		for i in xrange(100):
			if len(self.enemies) >= max_enemies or len(empty_blocks) < 1:
				break
			else:
				enemy_pos = empty_blocks[random.randint(0,len(empty_blocks) - 1)]
				if random.random() > 0.5:
					self.enemies.append([enemy_pos[0],enemy_pos[1],0,1])
				else:
					self.enemies.append([enemy_pos[0],enemy_pos[1],1,0])
				soln = self.can_reach(start_x,start_y,end_x,end_y,[0,0,0])
				#print(soln)
				if self.path_quality(soln) > 0 and len(soln) > len(old_soln):
					empty_blocks.remove(enemy_pos)
				else:
					self.enemies.pop()
					#print("no no")

		return True

	def print_board(self):
		#print("hello")
		"""for i in xrange(self.size_x):
			print("")
			for j in xrange(self.size_y):
				was_enemy = False
				for enemy in self.enemies:
					if enemy[0] == i and enemy[1] == j:
						if enemy[2] == 0:
							print(colored("h", self.color_to_string(self.grid[i][j][1])), end="")
						else:
							print(colored("v", self.color_to_string(self.grid[i][j][1])), end="")
						was_enemy = True
				if self.start_x == i and self.start_y == j:
					print(colored("S", self.color_to_string(self.grid[i][j][1])), end="")
					continue
				if self.end_x == i and self.end_y == j:
					print(colored("F", self.color_to_string(self.grid[i][j][1])), end="")
					continue
				if not was_enemy:
					print(colored(self.grid[i][j][0][0], self.color_to_string(self.grid[i][j][1])), end="") 
		print("")"""

	def color_to_string(self,color):
		if color == [1,0,0]:
			return 'red'
		elif color == [0,1,0]:
			return 'blue'
		elif color == [0,0,1]:
			return 'green'
		elif color == [1,1,0]:
			return 'magenta'
		elif color == [1,0,1]:
			return 'yellow'
		elif color == [0,1,1]:
			return 'cyan'
		elif color == [1,1,1]:
			return 'white'

	def export_print(self,text, f = None):
		if f is None:
			print(text,end = "")
		else:
			f.write(text)

	def export_level(self,f = None):
		self.export_print(str(self.size_x) + " " + str(self.size_y) + "\n",f)
		self.export_print(str(0)+"\n",f)
		for i in xrange(self.size_x):
			for j in xrange(self.size_y):
				was_enemy = False
				for enemy in self.enemies:
					if enemy[0] == i and enemy[1] == j:
						if enemy[2] == 0:
							self.export_print("h ",f)
						else:
							self.export_print("v ",f)
						was_enemy = True
				if was_enemy:
					continue
				if self.start_x == i and self.start_y == j:
					self.export_print("s ",f)
					continue
				if self.end_x == i and self.end_y == j:
					self.export_print("f ",f)
					continue
				if self.grid[i][j][0] == "empty" or self.grid[i][j][0] == "exit":
					self.export_print("e ",f)
				elif self.grid[i][j][0] == "block":
					self.export_print(str(self.color_to_string(self.grid[i][j][1])[0]) + " ",f)
				elif self.grid[i][j][0] == "switch":
					s = self.color_to_string(self.grid[i][j][1]).upper()
					self.export_print(s[0]+" ",f)
			self.export_print("\n",f)

	#colored bellman ford!

	def simulate_enemies(self, enemies, background_color):
		#enemies = copy.deepcopy(enemies)
		marked_for_death = []
		for enemy in enemies:
			if self.grid[enemy[0]][enemy[1]][0] == "block" and (self.grid[enemy[0]][enemy[1]][1] != background_color):
				marked_for_death.append(enemy)
				continue
			for i in xrange(2):
				new_x = enemy[0] + enemy[2]
				new_y = enemy[1] + enemy[3]
				if new_x > -1 and new_x < self.size_x and new_y > -1 and new_y < self.size_y:
					if self.grid[new_x][new_y][0] == "block" and (self.grid[new_x][new_y][1] != background_color):
						new_x = enemy[0]
						new_y = enemy[1]
						#if i==0:
						enemy[2] *= -1
						enemy[3] *= -1
						continue
				else:
					new_x = enemy[0]
					new_y = enemy[1]
					#if i==0:
					enemy[2] *= -1
					enemy[3] *= -1
					continue
				enemy[0] = new_x
				enemy[1] = new_y
				break
		for m in marked_for_death:
			#print("ready to die")
			enemies.remove(m)
		return enemies

	def can_reach(self,start_x, start_y, target_x, target_y, background_color):
		#t_start = time.clock()
		self.grid[target_x][target_y] = ["exit",[0,0,0]]
		marked_grid = []
		for i in xrange(self.size_x):
			row = []
			for j in xrange(self.size_y):
				row.append({})
			marked_grid.append(row)
		queue = []
		#heapq.heappush(queue,[0,0,0,tuple(background_color)])
		queue.append([0,start_x,start_y,tuple(background_color),copy.deepcopy(self.enemies)])
		enemy_tup = tuple(tuple(x) for x in list(self.enemies))
		marked_grid[start_x][start_y][(tuple(background_color),enemy_tup)] = 0
		#queue.append(marked_grid)
		found_path = False
		while len(queue) > 0:
			#position = heapq.heappop(queue)
			#print(queue)
			position = queue.pop(0)
			x = position[1]
			y = position[2]
			background_color = list(position[3])
			number = position[0]
			enemies = position[4]
			if found_path == True:
				break
			old_enemy_tup = tuple(tuple(x) for x in list(enemies))
			old_colr = position[3]
			"""if number > marked_grid[x][y][(tuple(background_color),enemy_tup)]:
				continue"""
			if x == target_x and y == target_y:
				found_path = True
				break
				#continue


			"""player_died = False
			for enemy in enemies:
				if enemy[0] == x and enemy[1] == y:
					player_died = True
					break
			if player_died:
				print ("ever happen??")
				continue"""

			#marked_grid[x][y].append(list(background_color))
			#add neighbors

			if self.grid[x][y][0] == "switch":
				#print("switched")
				for k in xrange(3):
					background_color[k] = (background_color[k] + self.grid[x][y][1][k]) % 2

			if number > 0:
				enemies = self.simulate_enemies(enemies, list(background_color))
			enemy_tup = tuple(tuple(x) for x in list(enemies))

			#similate enemies
			player_died = False
			for enemy in enemies:
				if enemy[0] == x and enemy[1] == y:
					player_died = True
					break
			if player_died:
				del marked_grid[x][y][(old_colr,old_enemy_tup)]
				continue

			for i in xrange(-1,2):
				for j in xrange(-1,2):
					if (i == 0 or j == 0) and x+i > -1 and x+i < self.size_x and y+j > -1 and y+j < self.size_y and not (i==0 and j==0):
						if (tuple(background_color),enemy_tup) in marked_grid[x+i][y+j].keys():
							#if number >= marked_grid[x+i][y+j][tuple(background_color)]:
							continue
						player_died = False
						for enemy in enemies:
							if enemy[0] == x + i and enemy[1] == y + j:
								#print("died")
								player_died = True
								break
						if player_died:
							#print("happens 2oo")
							continue
						if self.grid[x + i][y + j][0] == "empty":
							"""if x + i == target_x and y + j == target_y:
								return True"""
							marked_grid[x+i][y+j][(tuple(background_color),enemy_tup)] = number + 1
							queue.append([number + 1, x+i,y+j,tuple(background_color),copy.deepcopy(enemies)])
							#heapq.heappush(queue,[number + 1, x+i,y+j,tuple(background_color)])
						elif self.grid[x + i][y + j][0] == "exit":
							marked_grid[x+i][y+j][(tuple(background_color),enemy_tup)] = number + 1
							found_path = True
						elif self.grid[x + i][y + j][0] == "block":
							if background_color == self.grid[x + i][y + j][1]:
								queue.append([number + 1, x+i,y+j,tuple(background_color),copy.deepcopy(enemies)])
								#heapq.heappush(queue,[number + 1, x+i,y+j,tuple(background_color)])
								marked_grid[x+i][y+j][(tuple(background_color),enemy_tup)] = number + 1
						elif self.grid[x + i][y + j][0] == "switch":
							#toggle switch
							marked_grid[x+i][y+j][(tuple(background_color),enemy_tup)] = number + 1
							queue.append([number + 1, x+i,y+j,tuple(background_color),copy.deepcopy(enemies)])
							#heapq.heappush(queue,[number + 1, x+i,y+j,tuple(background_color)])
		"""if time.clock() - t_start > 0.5:
			print("uhoh:",time.clock() - t_start)"""
			#self.print_board()
		if not found_path:
			return []
		else:
			#countdown
			path = []
			x = target_x
			y = target_y
			num = 100000
			index = -1
			#print(marked_grid[x][y])
			#print("-*-*-*-")
			path.append([target_x,target_y])
			for key in marked_grid[x][y].keys():
				if marked_grid[x][y][key] < num:
					num = marked_grid[x][y][key]
					index = key
			#o_num = num
			#print("attempting to backtrack",num)
			while num > 0:
				found_next = False
				"""print("")
				print("")
				print("----------------")
				print("")"""
				ddBreak = False
				for i in xrange(-1,2):
					for j in xrange(-1,2):
						if (i == 0 or j == 0) and x+i > -1 and x+i < self.size_x and y+j > -1 and y+j < self.size_y and not (i==0 and j==0):
							#print("g:",marked_grid[x+i][y+j])
							#print("c:",index)
							colr = tuple(index[0])
							enemies = index[1]
							#oh god this is unclear
							keys = marked_grid[x+i][y+j].keys()
							future_keys = []
							translate_struct = {}
							for key in keys:
								enemy_list = list(list(x) for x in key[1])
								#enemies do not simulate on turn 0
								v = tuple(colr)
								if num > 1:
									f_enemies = self.simulate_enemies(enemy_list, list(v))
								else:
									f_enemies = enemy_list
								f_enemies = tuple(tuple(x) for x in f_enemies)
								future_keys.append((key[0], f_enemies))
								if not (key[0], f_enemies) in translate_struct:
									translate_struct[(key[0], f_enemies)] = []
								translate_struct[(key[0], f_enemies)].append((key[0],key[1]))

							if self.grid[x + i][y + j][0] == "switch":
								#print("is_switch")
								col = list(index[0])
								for k in xrange(3):
									col[k] = int((col[k] + self.grid[x + i][y + j][1][k]) % 2)
								colr = tuple(col)
								"""print(colr)
								print(colr in marked_grid[x+i][y+j].keys())
								print"""
							#print("*****")
							#print(marked_grid[x+i][y+i])
							"""if num == 1: 
								print("what I have:",(colr,enemies))
								print("what you have:",marked_grid[x+i][y+j])
								print((x+i,y+j))
								print((colr,enemies) in future_keys)
								if (colr,enemies) in future_keys:
									print(translate_struct)
									print(marked_grid[x+i][y+j])
									print(marked_grid[x+i][y+j][translate_struct[(colr,enemies)]])"""
							if (colr,enemies) in future_keys:
								#print(num,marked_grid[x+i][y+j][translate_struct[(colr,enemies)]])
								for translation in translate_struct[(colr,enemies)]:
									if marked_grid[x+i][y+j][translation] == num - 1:
										index = translation
										#print("index",index)
										x = x+i
										y = y+j
										#print("num:", num)
										path.append((x,y))
										num = num - 1
										"""if x == start_x and y == start_y:
											print ("path",path, num)"""
										#print(num,[x,y])
										#print("---------------------")
										ddBreak = True
										found_next = True
										break
								if ddBreak:
									break
					if ddBreak:
						break
				if not found_next:
					print("COULD NOT FIND NEXT:", num)
					break
			path.reverse()
			return path

if __name__ == "__main__":
    main()