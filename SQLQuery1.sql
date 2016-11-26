CREATE TABLE counters 
(
    influenser varchar(100), 
    tweet_type varchar(1),
	key_word varchar(100),
	kw_counter int
);
CREATE CLUSTERED INDEX i1 ON dbo.counters(influenser);

truncate table counters

select * from counters
select [kw_counter] from counters where [influenser] = 'test' and [tweet_type] = 'T' and [key_word] = 'google'

insert into [counters] ([influenser], [tweet_type], [key_word], [kw_counter]) values ('{0}', '{1}', '{2}', 1)

update [counters] set [kw_counter] = {3} where [influenser] = '{0}' and [tweet_type] = '{1}' and [key_word] = '{2}'


select top 1 [influenser] from counters where [key_word] = 'devops' order by [kw_counter] desc

select top 1 [key_word] from counters where [influenser] = 'Etnasoft' order by [kw_counter] desc

